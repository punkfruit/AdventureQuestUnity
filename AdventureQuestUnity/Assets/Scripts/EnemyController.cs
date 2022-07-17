using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum walkDirectionEnemy { North, South, West, East }
public enum IntendedDirection { None, North, South, East, West}
public enum EnemyType { Slime }
public class EnemyController : MonoBehaviour
{
    public int health;
    public Collider2D collid;
    public Transform trans;
    public bool isMoving;
    public Animator anim;
    public float moveSpeed;
    public SpriteRenderer spr;

    public Rigidbody2D theRB;
    public Sprite[] body;
    public walkDirectionEnemy walkDir;
    public EnemyType enType;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        WalkDirSpriteSwitch();

        //anim.SetBool("IsMoving", isMoving);
    }

    public void WalkDirSpriteSwitch()
    {
        switch (walkDir)
        {
            case walkDirectionEnemy.North:
                switch (enType)
                {
                    case EnemyType.Slime:
                        spr.sprite = body[0];
                        break;
                }
                break;
            case walkDirectionEnemy.East:
                switch (enType)
                {
                    case EnemyType.Slime:
                        spr.sprite = body[1];
                        break;
                }
                break;
            case walkDirectionEnemy.South:
                switch (enType)
                {
                    case EnemyType.Slime:
                        spr.sprite = body[2];
                        break;
                }
                break;
            case walkDirectionEnemy.West:
                switch (enType)
                {
                    case EnemyType.Slime:
                        spr.sprite = body[3];
                        break;
                }
                break;
        }
    }

    public void DamageEnemy(int damageToGive)
    {
        health -= damageToGive;
        if(health <= 0)
        {
            //die
        }
    }


}
