using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using System.Collections;
using Newtonsoft.Json;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string savePath;
    public bool allowedToSave = true;
    public bool allowedToLoad = true;
    public bool isLoadingFromSave = false;
    public Vector3? pendingPlayerPosition = null;



    


    [System.Serializable]
    public class ObjectStateEntry
    {
        public string id;
        public bool state;
    }

    [System.Serializable]
    public struct Vector3Data
    {
        public float x, y, z;

        public Vector3Data(Vector3 vec)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }

        public Vector3 ToVector3() => new Vector3(x, y, z);
    }

    [System.Serializable]
    public class GameData
    {
        public class QuestProgressEntry //inside gamedata, like this?
        {
            public int questID;
            public bool[] elementsCompleted;
        }
        
        public Vector3Data playerPosition;
        public string currentScene;
        public int maxHealth;
        public int playerHealth;
        public List<int> inventoryItems;
        public int[] inventoryItemCount;
        public List<ObjectStateEntry> objectStates = new();
        public List<QuestProgressEntry> activeQuests = new();
        
        public string chapter;           // <- NEW: human-readable chapter label ("Chapter 1")
        public string lastPlayed;          // e.g. "2025-09-09 14:05"
        public string thumbnailBase64;     // small screenshot as base64 (optional)
    }

    public GameData CurrentData = new();

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/save"; // Base path, we'll append slot
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame(int slotId = 1)
    {
        if (!allowedToSave)
        {
            Debug.LogWarning("Save blocked: already saving.");
            UIManager.Instance.ShowNotification("Already saving...", "!");
            return;
        }
        
        QuestManager.instance.SaveQuestProgress();
        
        // timestamp for the slot preview
        CurrentData.lastPlayed = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        if (GameManager.instance.CurrentLevelController != null)
        {
            CurrentData.chapter = GameManager.instance.CurrentLevelController.chapter;
        }

        // OPTIONAL: capture a tiny thumbnail (you can hook this up later)
        // CurrentData.thumbnailBase64 = SaveMenu.CaptureThumbnailBase64(256, 144);

#if UNITY_WEBGL
        if (!NGIO.isReady || !NGIO.hasUser)
        {
            Debug.LogWarning("NGIO not ready.");
            UIManager.Instance.ShowNotification("Not ready to save", "!");
            return;
        }

        allowedToSave = false;
        allowedToLoad = false;

        string json;
        try
        {
            json = JsonConvert.SerializeObject(CurrentData);
            JsonConvert.DeserializeObject<GameData>(json); // validate
        }
        catch (Exception e)
        {
            Debug.LogError("JSON validation failed: " + e.Message);
            UIManager.Instance.ShowNotification("Invalid save data", "!");
            allowedToSave = true;
            allowedToLoad = true;
            return;
        }

        Debug.Log($"[SaveGame] Writing cloud slot {slotId}, length: {json.Length}");
        Debug.Log(json);

        var saveComponent = new NewgroundsIO.components.CloudSave.setData()
        {
            id = slotId,
            data = json
        };

        StartCoroutine(NGIO.ngioCore.ExecuteComponent(saveComponent, (result) =>
        {
            if (result.success)
            {
                Debug.Log($"Save success (Slot {slotId})");
                UIManager.Instance.ShowNotification($"Saved Slot {slotId}", "!");
            }
            else
            {
                Debug.LogError($"Save FAILED (Slot {slotId}): " + result.error.message);
                UIManager.Instance.ShowNotification("Save failed!", "!");
            }

            allowedToSave = true;
            allowedToLoad = true;
        }));
#else
        string json = JsonConvert.SerializeObject(CurrentData, Formatting.Indented);
        string fullPath = $"{savePath}_{slotId}.json";
        File.WriteAllText(fullPath, json);
        Debug.Log($"Game Saved to: {fullPath}");
        UIManager.Instance.ShowNotification($"Game Saved (Slot {slotId})", "!");
#endif
        

    }

    public void LoadGame(int slotId = 1)
    {
#if UNITY_WEBGL
        
        if (!allowedToLoad)
        {
            Debug.LogWarning("Load blocked: save is still in progress.");
            UIManager.Instance.ShowNotification("Please wait... saving.", "!");
            return;
        }
        
        
        var loadComponent = new NewgroundsIO.components.CloudSave.loadSlot()
        {
            id = slotId
        };

        StartCoroutine(NGIO.ngioCore.ExecuteComponent(loadComponent, (result) =>
        {
            if (result.success)
            {
                var loadResult = result.result as NewgroundsIO.results.CloudSave.loadSlot;
                if (loadResult != null)
                {
                    var slot = loadResult.slot;
                    StartCoroutine(slot.GetData((data) =>
                    {
                        if (!string.IsNullOrEmpty(data))
                        {
                            try
                            {
                                Debug.Log("Raw loaded JSON:\n" + data); // <---- See it when it fails
                                CurrentData = JsonConvert.DeserializeObject<GameData>(data);
                                ApplyGameData();
                                Debug.Log($"Cloud load succeeded (Slot {slotId}).");
                                UIManager.Instance.ShowNotification($"Cloud Loaded (Slot {slotId})", "!");
                            }
                            catch (Exception e)
                            {
                                Debug.LogError("Failed to parse cloud save data: " + e.Message);
                                UIManager.Instance.ShowNotification("Corrupt save. Rewriting...", "!");

                                SaveGame(slotId); // attempt to fix by re-saving

                                allowedToLoad = true;
                            }
                        }
                        else
                        {
                            Debug.Log($"Slot {slotId} is empty.");
                            UIManager.Instance.ShowNotification("Slot Empty", "!");
                        }
                    }));
                }
                else
                {
                    Debug.LogError("Cloud load failed  could not cast result.");
                    UIManager.Instance.ShowNotification("Could not cast result", "!");
                }
            }
            else
            {
                Debug.LogError($"Cloud load FAILED (Slot {slotId}). Error: {result.error.message}");
                UIManager.Instance.ShowNotification("Fail, check console", "!");
            }
        }));
#else
        string fullPath = $"{savePath}_{slotId}.json";
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            CurrentData = JsonConvert.DeserializeObject<GameData>(json);
            ApplyGameData();
            Debug.Log($"Game Loaded from: {fullPath}");
            UIManager.Instance.ShowNotification($"Game Loaded (Slot {slotId})", "!");
        }
        else
        {
            Debug.Log("No local save file found.");
            UIManager.Instance.ShowNotification("No Save File Found", "!");
        }
#endif
    }

    private void ApplyGameData()
    {
        isLoadingFromSave = true;
        pendingPlayerPosition = CurrentData.playerPosition.ToVector3(); // Save it for later
    
        LevelManager.Instance.LoadLevelFromSave(CurrentData.currentScene); // Will trigger positioning after load

        // Defer these to avoid race conditions if their GameObjects aren't ready yet
        StartCoroutine(DeferredApplyData());
    }
    
    public IEnumerator DeferredApplyData()
    {
        yield return new WaitForSeconds(0.05f);
        // Give a few frames for scene objects to init

        InventoryManager.Instance.LoadInventoryByID(CurrentData.inventoryItems, CurrentData.inventoryItemCount);
        HealthManager.instance.LoadHealth(CurrentData.playerHealth, CurrentData.maxHealth);
        QuestManager.instance.LoadQuestProgress();

        GameManager.instance.onTitleScreen = false;
        UIManager.Instance.uiAccessible = true;
    }




    public void SetObjectState(string id, bool state)
    {
        var entry = CurrentData.objectStates.Find(e => e.id == id);
        if (entry != null)
        {
            entry.state = state;
        }
        else
        {
            CurrentData.objectStates.Add(new ObjectStateEntry { id = id, state = state });
        }
    }

    public bool GetObjectState(string id)
    {
        var entry = CurrentData.objectStates.Find(e => e.id == id);
        return entry != null && entry.state;
    }
}
