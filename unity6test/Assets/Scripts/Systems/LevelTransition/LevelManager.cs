using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public string targetTransition; // Name of the target transition area

    private Vector2 pendingOffset;
    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SaveManager.Instance.CurrentData.currentScene = SceneManager.GetActiveScene().name;
    }

    public void LoadNewLevel(string levelName, string targetTransitionArea, Vector2 offset)
    {
        // Only run this if not loading from a save
        if (SaveManager.Instance.isLoadingFromSave)
            return;

        targetTransition = targetTransitionArea;
        pendingOffset = offset;
        isTransitioning = true;

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(levelName);
    }

    public void LoadLevelFromSave(string levelName)
    {
        // SaveManager must set isLoadingFromSave to true before calling this
        isTransitioning = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(levelName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (SaveManager.Instance.isLoadingFromSave)
        {
            Debug.Log("Applying saved position from SaveManager.");
            PlayerController.instance.SetPlayerPosition(SaveManager.Instance.pendingPlayerPosition ?? Vector3.zero);
            SaveManager.Instance.pendingPlayerPosition = null;
            SaveManager.Instance.isLoadingFromSave = false;
            return;
        }

        if (!isTransitioning)
            return;

        isTransitioning = false;

        LevelTransition targetArea = FindTransitionArea(targetTransition);
        if (targetArea != null)
        {
            Vector3 targetPosition = targetArea.transform.position + (Vector3)pendingOffset;
            PlayerController.instance.SetPlayerPosition(targetPosition);
            SaveManager.Instance.CurrentData.currentScene = SceneManager.GetActiveScene().name;
            Debug.Log("Moved player from LevelManager");
        }
    }


    private LevelTransition FindTransitionArea(string targetName)
    {
        LevelTransition[] transitions = FindObjectsOfType<LevelTransition>();
        foreach (LevelTransition transition in transitions)
        {
            if (transition.gameObject.name == targetName)
                return transition;
        }

        Debug.LogWarning($"Target transition area '{targetName}' not found in the scene.");
        return null;
    }
}
