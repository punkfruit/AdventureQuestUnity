using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;

    public int health;
    public int maxHealth = 6;

    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    public GameObject heartPrefab; // Drag your heart prefab here
    public GameObject heartContainer; // Drag your grid layout container here

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

    void Start()
    {
        health = maxHealth;
        InitializeHearts();
        UpdateHealthDisplay();
        SaveManager.Instance.CurrentData.maxHealth = maxHealth;
        SaveManager.Instance.CurrentData.playerHealth = health;
    }

    private void InitializeHearts()
    {
        // Calculate initial number of hearts
        int heartCount = maxHealth / 2;

        // Clear any existing hearts
        foreach (Transform child in heartContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate hearts
        for (int i = 0; i < heartCount; i++)
        {
            Instantiate(heartPrefab, heartContainer.transform);
        }

        //UpdateHealthDisplay();
    }

    public void UpdateHealthDisplay()
    {
        // Calculate number of hearts
        int heartCount = maxHealth / 2;

        for (int i = 0; i < heartCount; i++)
        {
            Image heartImage = heartContainer.transform.GetChild(i).GetComponent<Image>();

            if (health >= (i + 1) * 2)
            {
                heartImage.sprite = fullHeart;
            }
            else if (health == (i * 2) + 1)
            {
                heartImage.sprite = halfHeart;
            }
            else
            {
                heartImage.sprite = emptyHeart;
            }
        }
        //Debug.Log("health is: " + health);
    }

    public void AddHeart()
    {
        maxHealth += 2;
        health = maxHealth;

        // Instantiate a new heart for the additional health
        Instantiate(heartPrefab, heartContainer.transform);

        UpdateHealthDisplay();
        SaveManager.Instance.CurrentData.maxHealth = maxHealth;
        SaveManager.Instance.CurrentData.playerHealth = health;
    }

    public bool TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        SaveManager.Instance.CurrentData.playerHealth = health;
        UpdateHealthDisplay();

        if (health <= 0)
        {
            
            //do something with the ui
            return true;
        }
        return false;
    }

    public void SetHealth(int value)
    {
        health = Mathf.Clamp(value, 0, maxHealth);
        UpdateHealthDisplay();
        SaveManager.Instance.CurrentData.playerHealth = health;
    }

    public void SetMaxHealth(int value)
    {
        maxHealth = value;
        InitializeHearts();
        UpdateHealthDisplay();
        SaveManager.Instance.CurrentData.maxHealth = maxHealth;
    }

    public void LoadHealth(int healthh, int maxHealthh)
    {
        maxHealth = maxHealthh;
        health = healthh;
        InitializeHearts();
        //UpdateHealthDisplay();
        StartCoroutine(DelayedUpdateHealthDisplay());
        
        PlayerController.instance.anim.SetBool("Dead", false);
        PlayerController.instance.canMove = true;
        PlayerController.instance.canMove = true;
        PlayerController.instance.playerState = PlayerStates.IDLE;
    }

    IEnumerator DelayedUpdateHealthDisplay()
    {
        yield return new WaitForSeconds(0.01f);
        UpdateHealthDisplay(); //i hate this lame coroutine, but if i dont have it then the health display doesnt update for some reason and i have no clue why ughhhhhhhhh
    }
}
