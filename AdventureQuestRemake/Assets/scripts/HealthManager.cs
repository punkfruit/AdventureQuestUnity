using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Sprite[] heartSprites;
    public Image[] hearts;


    public static HealthManager instance;

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

    public void HeartUpdate(int helth)
    {
        switch (helth)
        {
            case 1:
                hearts[0].sprite = heartSprites[0]; //0 = half
                hearts[1].sprite = heartSprites[2]; //1 = full
                hearts[2].sprite = heartSprites[2]; //2 = empty
                break;
            case 2:
                hearts[0].sprite = heartSprites[1];
                hearts[1].sprite = heartSprites[2];
                hearts[2].sprite = heartSprites[2];
                break;
            case 3:
                hearts[0].sprite = heartSprites[1];
                hearts[1].sprite = heartSprites[0];
                hearts[2].sprite = heartSprites[2];
                break;
            case 4:
                hearts[0].sprite = heartSprites[1];
                hearts[1].sprite = heartSprites[1];
                hearts[2].sprite = heartSprites[2];
                break;
            case 5:
                hearts[0].sprite = heartSprites[1];
                hearts[1].sprite = heartSprites[1];
                hearts[2].sprite = heartSprites[0];
                break;
            case 6:
                hearts[0].sprite = heartSprites[1];
                hearts[1].sprite = heartSprites[1];
                hearts[2].sprite = heartSprites[1];
                break;


            case 0:
                hearts[0].sprite = heartSprites[2];
                hearts[1].sprite = heartSprites[2];
                hearts[2].sprite = heartSprites[2];
                break;

        }
    }
}
