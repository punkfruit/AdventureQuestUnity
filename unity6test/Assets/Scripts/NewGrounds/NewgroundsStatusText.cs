using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewgroundsStatusText : MonoBehaviour
{
    public TextMeshProUGUI statusText;

    public void UpdateStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }
}
