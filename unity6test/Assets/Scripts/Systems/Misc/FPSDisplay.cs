using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    public Text display_Text;

    void Update()
    {
        if (display_Text != null)
        {
            int fps = (int)(1f / Time.deltaTime);
            display_Text.text = fps + " FPS";
        }
    }
}