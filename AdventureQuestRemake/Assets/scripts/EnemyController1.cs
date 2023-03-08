using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum walkDirectionEnemy1 { North, South, East, West }
public enum movementType { wander }
public enum enemyType { slime }
public class EnemyController1 : MonoBehaviour
{

    public int health;
    public Collider2D collid;
    public Transform trans;
    public Animator anim;
    public SpriteRenderer spr;
    public float moveSpeed;

    public Rigidbody2D theRB;
    public Sprite[] body;
    public walkDirectionEnemy1 walkDir;
    public enemyType enType;
    public GameObject deathAnim;
    public Transform deathAnimSpawn;
    public bool hitPlayer, playerInRange;
    public int contactDamage;
    public float contactDamageTimer;
    private float count1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        WalkDirSpriteSwitch();

        if (playerInRange)
        {
            if (count1 > 0)
            {
                count1 -= Time.deltaTime;
                hitPlayer = true;
            }
            else
            {
                hitPlayer = false;
                count1 = contactDamageTimer;
            }

            if (!hitPlayer)
            {
                PlayerController.instance.TakeDamage(contactDamage);
            }
        }
    }

    public void WalkDirSpriteSwitch()
    {
        switch (walkDir)
        {
            case walkDirectionEnemy1.North:
                switch (enType)
                {
                    case enemyType.slime:
                        spr.sprite = body[0];
                        break;
                }
                break;
            case walkDirectionEnemy1.East:
                switch (enType)
                {
                    case enemyType.slime:
                        spr.sprite = body[1];
                        spr.flipX = true; //might add if statement to see if this is a slime
                        break;
                }
                break;
            case walkDirectionEnemy1.South:
                switch (enType)
                {
                    case enemyType.slime:
                        spr.sprite = body[2];
                        break;
                }
                break;
            case walkDirectionEnemy1.West:
                switch (enType)
                {
                    case enemyType.slime:
                        spr.sprite = body[3];
                        spr.flipX = false; //might add if statement to see if this is a slime
                        break;
                }
                break;
        }
    }

    public void DamageEnemy(int damageToGive)
    {
        health -= damageToGive;
        anim.SetTrigger("EnemyHit");
        if(health <= 0)
        {
            //die
            Instantiate(deathAnim, deathAnimSpawn.transform.position, deathAnimSpawn.transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
            count1 = 0;
            //Debug.Log("playerLeft");
        }
    }

}
