using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class NGIOManager : MonoBehaviour
{
    [Header("Newgrounds Info")]
    public string appID = "YOUR_APP_ID";
    public string aesKey = "YOUR_AES_KEY";

    [Header("UI Events")]
    public UnityEvent<string> OnNewgroundsStatusChanged;

    private void Start()
    {
        var options = new Dictionary<string, object>()
        {
            { "version", "1.0.0" },
            { "checkHostLicense", true },
            { "autoLogNewView", true },
            { "preloadMedals", true },
            { "preloadScoreBoards", false },
            { "preloadSaveSlots", true }
        };

        NGIO.Init(appID, aesKey, options);
    }

    private void Update()
    {
        StartCoroutine(NGIO.GetConnectionStatus(OnConnectionStatusChanged));
    }

    void OnConnectionStatusChanged(string status)
    {
        if (!NGIO.legalHost) return;

        switch (status)
        {
            case NGIO.STATUS_READY:
                if (NGIO.hasUser)
                {
                    Debug.Log("Logged in as: " + NGIO.user.name);
                    OnNewgroundsStatusChanged?.Invoke("Welcome, " + NGIO.user.name + "!");
                }
                else
                {
                    OnNewgroundsStatusChanged?.Invoke("Not logged in.");
                }
                break;

            case NGIO.STATUS_LOGIN_REQUIRED:
                OnNewgroundsStatusChanged?.Invoke("Login required...");
                NGIO.OpenLoginPage();
                break;

            default:
                OnNewgroundsStatusChanged?.Invoke("Connecting to Newgrounds...");
                break;
        }
    }

}
