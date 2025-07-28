using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public Button PlayButton;
    public string firstArea = "Area01";


    private void Start()
    {
        PlayButton.Select();
    }

    public void StartGame()
    {
        GameManager.instance.StartGame(firstArea);
    }
}
