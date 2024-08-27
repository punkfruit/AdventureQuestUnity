using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    public Color flashColor;

    public bool followPlayer;
    public float attackDelay = 0.3f;
    public float wanderDuration = 2.0f;   // How long the enemy should wander in a chosen direction
    public float waitDuration = 1.0f;     // How long the enemy should wait before picking a new direction
    private bool isWandering = false;     // Flag to check if the enemy is currently wandering
    public float wanderSpeed;

    private void Start()
    {
        spr.material.SetColor("_FlashColor", flashColor);
    }

    void FixedUpdate()
    {
        if (followPlayer)
        {
            Follow();
        }
        else if (!isWandering && !isAttacking)
        {
            StartCoroutine(Wander());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            followPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            followPlayer = false;
        }
    }

    private void Update()
    {

        spr.material.SetFloat("_BlendOpacity", flash);
    }

    public void Follow()
    {
        if(PlayerController.instance.transform.position.x > transform.position.x)
        {
            spr.flipX = false;
        }
        else
        {
            spr.flipX = true;
        }

        Vector2 directionToPlayer = PlayerController.instance.transform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (!isAttacking)
        {
            if (distanceToPlayer > stopDistance)
            {
                directionToPlayer.Normalize();
                theRB.velocity = directionToPlayer * walkSpeed;
            }
            else
            {
                theRB.velocity = Vector2.zero;
                lastKnownPlayerPosition = PlayerController.instance.transform.position;
                StartAttack();
            }
        }


        void StartAttack()
        {
            isAttacking = true;
            StartCoroutine(AttackMovement());
        }

        IEnumerator AttackMovement()
        {
            yield return new WaitForSeconds(attackDelay);  // Introduce a delay of 0.5 seconds

            Vector2 attackDirection = (lastKnownPlayerPosition - (Vector2)transform.position).normalized;
            float attackDuration = 0.5f;  // You can adjust this value based on your needs

            anim.SetTrigger("JumpAttack");

            float startTime = Time.time;

            // This loop will move the enemy towards the last known player position over a short duration
            while (Time.time < startTime + attackDuration)
            {
                theRB.velocity = attackDirection * attackSpeed;
                yield return null;  // Wait for the next frame
            }

            theRB.velocity = Vector2.zero;  // Stop the enemy
            isAttacking = false;
        }

    }


    IEnumerator Wander()
    {
        isWandering = true;

        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        // Flip the sprite based on the wander direction
        if (randomDirection.x > 0)
        {
            spr.flipX = false;  // Moving right
        }
        else
        {
            spr.flipX = true;   // Moving left
        }

        float wanderStartTime = Time.time;

        while (Time.time < wanderStartTime + wanderDuration)
        {
            theRB.velocity = randomDirection * wanderSpeed;
            yield return null;
        }

        theRB.velocity = Vector2.zero;

        // Make the slime wait for a while before wandering again
        yield return new WaitForSeconds(waitDuration);

        isWandering = false;
    }


}
