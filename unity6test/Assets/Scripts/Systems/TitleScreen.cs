using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public Button PlayButton, aboutButton;
    public string firstArea = "Area01";

    public GameObject buttonHolder, buttonHolder2, titlePanel;
    public Button newGameButton, loadGameButton;


    private void Start()
    {
        PlayButton.Select();
        buttonHolder2.SetActive(false);

        GameManager.instance.onTitleScreen = true;
    }

    public void StartGame()
    {
        GameManager.instance.StartGame(firstArea);
    }

    public void PlayButtonPressed() //show small menu asking if load or start new game
    {
        buttonHolder2.SetActive(true);
        buttonHolder.SetActive(false);
        newGameButton.Select();
    }

    public void LoadButtonPressed()
    {
        //UIManager.Instance.PauseGame();
        buttonHolder.SetActive(false);
        titlePanel.SetActive(false);
        
        UIManager.Instance.pauseMenu.SetActive(true);
        UIManager.Instance.ShowSaveMenu();
        //Debug.Log("Load button pressed");
    }
}
