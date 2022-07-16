using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public bool weapon = false;
    public bool hit = false;
    public int damage;
    public SpriteRenderer spr;
    // Start is called before the first frame update
    void Start()
    {
        if (weapon)
        {
            switch (PlayerController.instance.walkdir)
            {
                case walkDirection.North:
                    spr.sortingOrder = -5;
                    break;
                case walkDirection.East:
                    spr.sortingOrder = -5;
                    break;
                case walkDirection.South:
                    spr.sortingOrder = 5;
                    break;
                case walkDirection.West:
                    spr.sortingOrder = -5;
                    break;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (weapon)
        {
            if (other.tag == "Enemy")
            {
                if (!hit)
                {
                    other.GetComponent<EnemyController>().DamageEnemy(damage);
                    hit = true;
                }

            }
        }
    }


    public void SelfDestruct()
    {
        Destroy(gameObject);
        PlayerController.instance.canSwing = true;
    }
}
