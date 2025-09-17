using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SaveMenuButton : MonoBehaviour
{
    [Header("Slot")]
    public int slotId = 1;

    [Header("UI Refs")]
    public TMP_Text titleText;            // "Chapter 1" or scene name
    public TMP_Text subtitleText;         // "Save Slot 1 • Last played ..."
    public RawImage thumbnail;
    public Transform heartsContainer;     // parent under "Hearts" GridLayoutGroup
    public GameObject heartPrefab;        // heart icon prefab
    public Button saveButton;
    public Button loadButton;
    public Button deleteButton;

    // data cache
    private bool hasData = false;

    private void Awake()
    {
        if (saveButton)  saveButton.onClick.AddListener(OnClickSave);
        if (loadButton)  loadButton.onClick.AddListener(OnClickLoad);
        if (deleteButton) deleteButton.onClick.AddListener(OnClickDelete);
    }

    // Called by SaveMenu when it knows this slot is empty
    public void ShowEmpty()
    {
        hasData = false;

        if (titleText)    titleText.text = "Empty";
        if (subtitleText) subtitleText.text = $"Save Slot {slotId}";

        if (thumbnail)    thumbnail.gameObject.SetActive(false);
        ClearHearts();

        if (loadButton)   loadButton.interactable = false;
        if (deleteButton) deleteButton.interactable = false;
    }

    // Called by SaveMenu with preview data
    public void ShowPreview(SavePreview preview)
    {
        hasData = true;

        if (titleText)
            titleText.text = string.IsNullOrEmpty(preview.chapter) ? "Unknown Chapter" : preview.chapter;


        if (subtitleText)
        {
            var last = string.IsNullOrEmpty(preview.lastPlayed) ? "" : $" • {preview.lastPlayed}";
            subtitleText.text = $"Save Slot {slotId}{last}";
        }

        if (thumbnail)
        {
            if (preview.thumbnail != null)
            {
                thumbnail.texture = preview.thumbnail;
                thumbnail.gameObject.SetActive(true);
            }
            else
            {
                thumbnail.gameObject.SetActive(false);
            }
        }

        // Hearts: show current player health as half-hearts count
        RenderHearts(preview.playerHealth / 2);

        if (loadButton)   loadButton.interactable = true;
        if (deleteButton) deleteButton.interactable = true;
    }

    private void RenderHearts(int count)
    {
        ClearHearts();
        if (!heartPrefab || !heartsContainer) return;

        for (int i = 0; i < Mathf.Max(0, count); i++)
        {
            Instantiate(heartPrefab, heartsContainer);
        }
    }

    private void ClearHearts()
    {
        if (!heartsContainer) return;
        for (int i = heartsContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(heartsContainer.GetChild(i).gameObject);
        }
    }

    // Buttons
    public void OnClickSave()
    {
        SaveManager.Instance.SaveGame(slotId);
        UIManager.Instance.UnPauseGame();
    }

    public void OnClickLoad()
    {
        if (SaveManager.Instance.allowedToLoad)
        {
            SaveManager.Instance.LoadGame(slotId);
            UIManager.Instance.UnPauseGame();
        }
    }

    public void OnClickDelete()
    {
#if UNITY_WEBGL
        if (!this) return;

        if (deleteButton) deleteButton.interactable = false;

        StartCoroutine(DeleteCloudSlotRoutine());
#else
        string fullPath = $"{Application.persistentDataPath}/save_{slotId}.json";
        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
            ShowEmpty();
            UIManager.Instance.ShowNotification($"Cleared Slot {slotId}", "!");
        }
#endif
    }

#if UNITY_WEBGL
    private IEnumerator DeleteCloudSlotRoutine()
    {
        int attempts = 0;
        const int maxAttempts = 2; // one retry on transient transport error
        bool success = false;

        while (attempts < maxAttempts && !success)
        {
            attempts++;

            var clear = new NewgroundsIO.components.CloudSave.clearSlot() { id = slotId };
            bool finished = false;
            bool callSuccess = false;

            yield return NGIO.ngioCore.ExecuteComponent(clear, (result) =>
            {
                if (!this) return; // row destroyed
                callSuccess = (result != null && result.success);
                finished = true;
            });

            int safety = 0;
            while (!finished && safety++ < 120) yield return null;

            success = callSuccess;

            if (!success)
            {
                // Small backoff for hiccups like curl 65
                yield return new WaitForSecondsRealtime(0.3f);
            }
        }

        if (this)
        {
            if (success)
            {
                ShowEmpty();
                UIManager.Instance.ShowNotification($"Cleared Slot {slotId}", "!");

                // Ask menu to refresh after a short delay; its cooldown avoids stale reads
                if (SaveMenu.Instance)
                {
                    SaveMenu.Instance.Invoke(nameof(SaveMenu.Instance.RefreshAllSlots), 0.15f);
                }
            }
            else
            {
                UIManager.Instance.ShowNotification("Delete failed — try again", "!");
            }

            if (deleteButton) deleteButton.interactable = true;
        }
    }
#endif
}
