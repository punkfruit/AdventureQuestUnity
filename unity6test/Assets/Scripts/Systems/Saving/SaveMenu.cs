using UnityEngine;
using System;
using System.Collections;
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

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        RefreshAllSlots();
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
#if UNITY_WEBGL
            StartCoroutine(RefreshCloudSlot(row, slotId));
#else
            RefreshLocalSlot(row, slotId);
#endif
        }
    }

#if UNITY_WEBGL
    private IEnumerator RefreshCloudSlot(SaveMenuButton row, int slotId)
    {
        if (row == null) yield break;

        var load = new NewgroundsIO.components.CloudSave.loadSlot() { id = slotId };
        bool callbackReturned = false;
        NewgroundsIO.objects.SaveSlot slot = null;

        // Kick off loadSlot
        yield return NGIO.ngioCore.ExecuteComponent(load, (response) =>
        {
            if (response.success &&
                response.result is NewgroundsIO.results.CloudSave.loadSlot lr &&
                lr.slot != null &&
                lr.slot.hasData)
            {
                slot = lr.slot;
            }
            callbackReturned = true;
        });

        // Wait for the callback to return
        while (!callbackReturned) yield return null;

        // Early out if row/menu got destroyed/disabled
        if (this == null || !isActiveAndEnabled || row == null || !row.isActiveAndEnabled)
            yield break;

        if (slot == null || !slot.hasData)
        {
            row.ShowEmpty();
            yield break;
        }

        // Fetch the slot data on THIS MonoBehaviour (not the row) to avoid NREs if row disappears
        yield return GetDataAndShow(row, slot);
    }

    private IEnumerator GetDataAndShow(SaveMenuButton row, NewgroundsIO.objects.SaveSlot slot)
    {
        bool done = false;
        string json = null;

        // Request the string
        yield return slot.GetData((data) =>
        {
            json = data;
            done = true;
        });

        while (!done) yield return null;

        if (this == null || !isActiveAndEnabled || row == null || !row.isActiveAndEnabled)
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
                sceneName = string.IsNullOrEmpty(data.currentScene) ? "Unknown Area" : data.currentScene,
                chapter   = null, // set when you add a chapter field
                lastPlayed = data.lastPlayed,
                playerHealth = data.playerHealth,
                thumbnail = DecodeBase64Texture(data.thumbnailBase64)
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
