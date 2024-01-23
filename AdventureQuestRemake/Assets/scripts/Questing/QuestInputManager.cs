using MoreMountains.InventoryEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class QuestInputManager : MonoBehaviour, MMEventListener<MMGameEvent>
{
    //public CanvasGroup TargetQuestContainer;
    public GameObject QuestMenu;
    public QuestManager Qman;


    public InputActionProperty ToggleQuestMenuKey;
    public InputActionProperty CancelKey;
    public InputActionProperty ActionKey;



    public GameObject _currentSelection;
    public InventorySlot CurrentlySelectedQuest;
    

    public InventoryInputManager inventoryInputManager;
    public ScrollRectAutoScroll scroller;
    public WaitForSeconds seconds;

    private void Start()
    {
        //DontDestroyOnLoad(this);
        CloseQuestMenu();

        inventoryInputManager = FindObjectOfType<InventoryInputManager>();

        seconds = new WaitForSeconds(0.05f);
    }

    private void OnEnable()
    {
        ToggleQuestMenuKey.action.Enable();
        CancelKey.action.Enable();
        ActionKey.action.Enable();

        ToggleQuestMenuKey.action.performed += HandleToggleQuestMenu;
        CancelKey.action.performed += HandleCloseQuestMenu;


        this.MMEventStartListening<MMGameEvent>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDestroy()
    {
        ToggleQuestMenuKey.action.Disable();
        CancelKey.action.Disable();
        ActionKey.action.Disable();

        ToggleQuestMenuKey.action.performed -= HandleToggleQuestMenu;
        CancelKey.action.performed -= HandleCloseQuestMenu;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reassign references here
        Qman = FindObjectOfType<QuestManager>();
        inventoryInputManager = FindObjectOfType<InventoryInputManager>();
        // Add similar lines for other references that might be lost on scene load
    }





    private void HandleToggleQuestMenu(InputAction.CallbackContext context)
    {
        ToggleQuestMenu();
    }

    public void ToggleQuestMenu() 
    {

        if(Qman.QuestMenuOpen)
        {
            CloseQuestMenu();
        }
        else
        {
            OpenQuestMenu();
        }
    }

    public void OpenQuestMenu()
    {

        if (Qman == null)
        {
            Debug.Log("Qman is null, attempting to find QuestManager.");
            Qman = FindObjectOfType<QuestManager>();
            if (Qman == null) Debug.LogError("Failed to find QuestManager.");
        }
        if (inventoryInputManager == null)
        {
            inventoryInputManager = FindObjectOfType<InventoryInputManager>();
        }

        if (Qman.canOpenQuestMenu)
        {
            QuestMenu.SetActive(true);
            Qman.QuestMenuOpen = true;
            inventoryInputManager.canOpenInventory = false;
            PlayerController.instance.canMove = false;
            //scroller.enabled = true;
            StartCoroutine(TurnOnScroller());
            if (Qman != null)
            {
                Qman.RefreshQuestUI();
            }
        }
        
    }

    public void CloseQuestMenu()
    {

        if (Qman == null)
        {
            Debug.Log("Qman is null, attempting to find QuestManager.");
            Qman = FindObjectOfType<QuestManager>();
            if (Qman == null) Debug.LogError("Failed to find QuestManager.");
        }
        if (inventoryInputManager == null)
        {
            inventoryInputManager = FindObjectOfType<InventoryInputManager>();
        }

        QuestMenu.SetActive(false);
        Qman.QuestMenuOpen = false;
        inventoryInputManager.canOpenInventory = true;
        PlayerController.instance.canMove = true;
        scroller.enabled = false;

    }


    private void HandleCloseQuestMenu(InputAction.CallbackContext context)
    {
        CloseQuestMenu();
    }













    protected virtual void OnDisable()
    {
        this.MMEventStopListening<MMGameEvent>();
    }

    public void OnMMEvent(MMGameEvent gameEvent)
    {
        if (gameEvent.EventName == "inventoryOpens")
        {
            Qman.canOpenQuestMenu = false;
        }

        if (gameEvent.EventName == "inventoryCloses")
        {
            Qman.canOpenQuestMenu = true;
        }
    }

    public IEnumerator TurnOnScroller()
    {
        yield return seconds;

        scroller.enabled = true;

        yield return seconds;

        scroller.ScrollToSelected(false);
    }
}
