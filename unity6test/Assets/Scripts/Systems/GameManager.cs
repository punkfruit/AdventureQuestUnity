using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerInput playerInput;
    public PlayerController player;

    public GameObject uiCanvas;
    
    public bool onTitleScreen = false;

    public LevelController CurrentLevelController;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this);

        if (!onTitleScreen)
        {
            HealthManager.instance.heartContainer.SetActive(true);
        }
    }

    

    public void OnMove(InputValue movementValue)
    {
        if (player != null)
        {
            Vector2 move = movementValue.Get<Vector2>();
            player.SetMoveInput(move);
        }
    }

    public void OnFire(InputValue fireValue)
    {
        if (player != null)
        {
            player.TryFire();
        }
    }

    public void StartGame(string FirstArea)
    {
        //uiCanvas.SetActive(true);
        HealthManager.instance.heartContainer.SetActive(true);
        LevelManager.Instance.LoadLevelFromSave(FirstArea);
    }

    public void ShowCanvas()
    {
        uiCanvas.SetActive(true);
    }

}
