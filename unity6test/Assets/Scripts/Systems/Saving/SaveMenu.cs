using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class SavePreview
{
    public string sceneName;
    public string chapter;      // optional future field
    public string lastPlayed;   // from GameData
    public int playerHealth;
    public Texture2D thumbnail; // decoded from base64 (optional)
}

public class SaveMenu : MonoBehaviour
{
    public static SaveMenu Instance;

    [Header("Config")]
    public int totalSlots = 3;
    public SaveMenuButton[] slotRows; // assign in Inspector

    // Robustness state
    private int menuVersion = 0;
    private readonly Dictionary<int, bool> slotRefreshInFlight = new();
    private readonly Dictionary<int, float> slotCooldownUntil = new();
    private const float SlotCooldownSeconds = 0.5f;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        menuVersion++;
        // Try immediately; if NGIO isn’t ready yet, we start a small waiter
        RefreshAllSlots();
#if UNITY_WEBGL
        StartCoroutine(WaitForNgioAndRefresh(menuVersion));
#endif
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        slotRefreshInFlight.Clear();
        slotCooldownUntil.Clear();
    }

    public void RefreshAllSlots()
    {
        if (!isActiveAndEnabled) return;
        if (slotRows == null || slotRows.Length == 0) return;

        for (int i = 0; i < slotRows.Length; i++)
        {
            var row = slotRows[i];
            if (row == null) continue;

            int slotId = row.slotId;

            // no duplicate refreshes for same slot
            if (slotRefreshInFlight.TryGetValue(slotId, out var busy) && busy) continue;
            if (slotCooldownUntil.TryGetValue(slotId, out var until) && Time.unscaledTime < until) continue;

            if (GameManager.instance != null)
            {
                if (GameManager.instance.onTitleScreen)
                {
                    row.saveButton.enabled = false; //should break the save button on the title screen mayhaps
                }
            }
            else
            {
                Debug.LogError("Game Manager not found");
            }
            

#if UNITY_WEBGL
            // If NGIO not ready yet, show empty for now (prevents NRE) — waiter will repopulate when ready.
            if (!IsNgioCloudReady())
            {
                row.ShowEmpty();
                continue;
            }
            StartCoroutine(RefreshCloudSlot(row, slotId, menuVersion));
#else
            RefreshLocalSlot(row, slotId);
#endif
        }
    }

#if UNITY_WEBGL
    /// <summary>Polls briefly for NGIO readiness, then refreshes once ready.</summary>
    private IEnumerator WaitForNgioAndRefresh(int version)
    {
        // Fast path: if already ready, bail.
        if (IsNgioCloudReady()) yield break;

        // Poll for up to ~3 seconds without spamming
        float start = Time.unscaledTime;
        while (Time.unscaledTime - start < 3f)
        {
            if (!this || !isActiveAndEnabled || version != menuVersion) yield break;
            if (IsNgioCloudReady())
            {
                RefreshAllSlots();
                yield break;
            }
            yield return new WaitForSecondsRealtime(0.15f);
        }
        // If still not ready after 3s, we stay showing “Empty” until user reopens menu or NGIO becomes ready later.
    }

    /// <summary>Returns true when it’s safe to hit CloudSave.</summary>
    private bool IsNgioCloudReady()
    {
        // ngioCore must exist, wrapper initialized, and a logged-in user (CloudSave requires a session)
        return NGIO.isInitialized
            && NGIO.ngioCore != null
            && NGIO.hasUser; // (You could also allow NGIO.hasSession, but CloudSave requires login.)
    }

    private IEnumerator RefreshCloudSlot(SaveMenuButton row, int slotId, int version)
    {
        if (row == null || !isActiveAndEnabled) yield break;

        slotRefreshInFlight[slotId] = true;
        try
        {
            // Double-check readiness in case state changed after we queued
            if (!IsNgioCloudReady())
            {
                row.ShowEmpty();
                yield break;
            }

            var load = new NewgroundsIO.components.CloudSave.loadSlot() { id = slotId };

            bool callbackReturned = false;
            NewgroundsIO.objects.SaveSlot slot = null;

            // Execute with a callback; response can be null on transport error
            yield return NGIO.ngioCore.ExecuteComponent(load, (response) =>
            {
                if (this == null || !isActiveAndEnabled || version != menuVersion) return;

                if (response != null &&
                    response.success &&
                    response.result is NewgroundsIO.results.CloudSave.loadSlot lr &&
                    lr.slot != null &&
                    lr.slot.hasData)
                {
                    slot = lr.slot;
                }
                callbackReturned = true;
            });

            // wait for callback (defensive)
            int safety = 0;
            while (!callbackReturned && safety++ < 120) yield return null;

            if (this == null || !isActiveAndEnabled || version != menuVersion || row == null || !row.isActiveAndEnabled)
                yield break;

            if (slot == null || !slot.hasData)
            {
                row.ShowEmpty();
                yield break;
            }

            // Fetch JSON string on THIS MonoBehaviour so if the row disappears, we don't NRE
            yield return GetDataAndShow(row, slot, version);
        }
        finally
        {
            slotRefreshInFlight[slotId] = false;
            slotCooldownUntil[slotId] = Time.unscaledTime + SlotCooldownSeconds;
        }
    }

    private IEnumerator GetDataAndShow(SaveMenuButton row, NewgroundsIO.objects.SaveSlot slot, int version)
    {
        bool done = false;
        string json = null;

        yield return slot.GetData((data) =>
        {
            json = data;
            done = true;
        });

        int safety = 0;
        while (!done && safety++ < 120) yield return null;

        if (this == null || !isActiveAndEnabled || version != menuVersion || row == null || !row.isActiveAndEnabled)
            yield break;

        if (string.IsNullOrEmpty(json))
        {
            row.ShowEmpty();
            yield break;
        }

        var preview = BuildPreview(json);
        if (preview != null) row.ShowPreview(preview);
        else row.ShowEmpty();
    }
#else
    private void RefreshLocalSlot(SaveMenuButton row, int slotId)
    {
        string path = $"{Application.persistentDataPath}/save_{slotId}.json";
        if (!System.IO.File.Exists(path))
        {
            row.ShowEmpty();
            return;
        }

        try
        {
            var json = System.IO.File.ReadAllText(path);
            var preview = BuildPreview(json);
            if (preview != null) row.ShowPreview(preview);
            else row.ShowEmpty();
        }
        catch
        {
            row.ShowEmpty();
        }
    }
#endif

    private SavePreview BuildPreview(string json)
    {
        try
        {
            var data = JsonConvert.DeserializeObject<SaveManager.GameData>(json);
            if (data == null) return null;

            return new SavePreview
            {
                sceneName    = string.IsNullOrEmpty(data.currentScene) ? "Unknown Area" : data.currentScene,
                chapter      = string.IsNullOrEmpty(data.chapter) ? "Unknown Chapter" : data.chapter,
                lastPlayed   = data.lastPlayed,
                playerHealth = data.playerHealth,
                thumbnail    = DecodeBase64Texture(data.thumbnailBase64)
            };
        }
        catch
        {
            return null;
        }
    }

    private Texture2D DecodeBase64Texture(string base64)
    {
        if (string.IsNullOrEmpty(base64)) return null;
        try
        {
            byte[] bytes = Convert.FromBase64String(base64);
            var tex = new Texture2D(2, 2, TextureFormat.RGB24, false);
            tex.LoadImage(bytes);
            return tex;
        }
        catch { return null; }
    }

    // Optional: capture thumbnail here instead of SaveManager
    public static string CaptureThumbnailBase64(int width, int height)
    {
        var tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tex.Apply();

        var small = ScaleTexture(tex, width, height);
        var jpg = small.EncodeToJPG(60);
        UnityEngine.Object.Destroy(tex);
        UnityEngine.Object.Destroy(small);
        return Convert.ToBase64String(jpg);
    }

    private static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        var rt = RenderTexture.GetTemporary(targetWidth, targetHeight, 0, RenderTextureFormat.ARGB32);
        Graphics.Blit(source, rt);
        var prev = RenderTexture.active;
        RenderTexture.active = rt;
        var result = new Texture2D(targetWidth, targetHeight, TextureFormat.RGB24, false);
        result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        result.Apply();
        RenderTexture.active = prev;
        RenderTexture.ReleaseTemporary(rt);
        return result;
    }
}
