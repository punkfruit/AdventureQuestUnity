using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum walkDirectionEnemy1 { North, South, East, West }
public enum spriteDirectionEnemy { North, South, East, West }
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
    }
}
