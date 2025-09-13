using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SaveMenuButton : MonoBehaviour
{
    [Header("Slot")]
    public int slotId = 1;

    [Header("UI Refs")]
    public TMP_Text titleText;            // e.g. "Chapter 1" or scene name
    public TMP_Text subtitleText;         // e.g. "Save Slot 1" or "Last played"
    public RawImage thumbnail;
    public Transform heartsContainer;     // empty parent under your "Hearts" GridLayoutGroup
    public GameObject heartPrefab;        // a small heart icon prefab
    public Button saveButton;
    public Button loadButton;
    public Button deleteButton;

    // data cache
    private bool hasData = false;

    private void Awake()
    {
        // default wiring (optional)
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

        // Title: use "chapter" if you later add it; for now, show scene or your placeholder
        if (titleText)    titleText.text = string.IsNullOrEmpty(preview.chapter) ? preview.sceneName : preview.chapter;

        // Subtitle: show both slot label and last played if available
        if (subtitleText)
        {
            var last = string.IsNullOrEmpty(preview.lastPlayed) ? "" : $" • {preview.lastPlayed}";
            subtitleText.text = $"Save Slot {slotId}{last}";
        }

        // Thumbnail
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

        // Hearts = current player health
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
        var clear = new NewgroundsIO.components.CloudSave.clearSlot() { id = slotId };

        StartCoroutine(NGIO.ngioCore.ExecuteComponent(clear, (result) =>
        {
            if (!this) return; // button was destroyed/row closed

            if (result.success)
            {
                ShowEmpty();
                UIManager.Instance.ShowNotification($"Cleared Slot {slotId}", "!");
            }
            else
            {
                UIManager.Instance.ShowNotification("Delete failed", "!");
            }
        }));
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

}
