using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float flash = 0;
    public Renderer rend;
    public SpriteRenderer spr;
    public Animator anim;
    public Rigidbody2D theRB;
    public float walkSpeed;
    public int health = 5;
    public float stopDistance = 2.0f;  // Set to whatever distance you want.
    public float attackSpeed = 10.0f;  // Speed at which the enemy "jumps" toward the player during the attack.
    public bool isAttacking = false;   // A flag to check if the enemy is currently in the attack mode.
    public Vector2 lastKnownPlayerPosition;



    


    public void TakeDamage(int dam)
    {
        health -= dam;
    }


}
