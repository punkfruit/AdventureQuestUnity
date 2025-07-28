using UnityEngine;
using System.Collections;

public enum FacingDir { UP, DOWN, LEFT, RIGHT }
public enum EnemyStates { IDLE, MOVE, ATTACK, STUN }
public class Enemy : MonoBehaviour
{
    public int health = 5;
    public FacingDir Direction = FacingDir.DOWN;
    public EnemyStates State = EnemyStates.IDLE;
    
    public GameObject deathEffect;

    public float moveSpeed = 2f;
    public float minMoveTime = 1f;
    public float maxMoveTime = 3f;
    public float minWaitTime = 0.5f;
    public float maxWaitTime = 2f;
    
    

    public float knockbackForce = 5f; // Adjust this value for knockback strength

    [SerializeField] private Vector2 direction; 
    private Rigidbody2D rb;
    public Animator anim;

    public Collider2D hitbox;

    private void Start()
    {
        // Get the Rigidbody2D component attached to the enemy
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(Wander());
    }

    private void Update()
    {
        // Movement happens in FixedUpdate (not Update) when using physics
        HandleAnimation();
    }

    public void HandleAnimation()
    {
        anim.SetFloat("dirX", direction.x);
        anim.SetFloat("dirY", direction.y);
    }
    private void FixedUpdate()
    {
        if(State != EnemyStates.STUN)
        {
            // Move the enemy while in the RUN state
            if (State == EnemyStates.MOVE)
            {
                // Move using Rigidbody2D.MovePosition to respect physics and collisions
                //rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                rb.linearVelocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
            }
            else if (State == EnemyStates.IDLE)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
       
    }

    // Call this method when the player attacks the enemy
    public void ApplyKnockback(Vector2 attackDirection)
    {
        hitbox.enabled = false;
        StopAllCoroutines();
        
        State = EnemyStates.STUN;

        // Apply force to the Rigidbody in the direction of the attack
        rb.AddForce(attackDirection * knockbackForce, ForceMode2D.Impulse);

        // Optionally, switch back to kinematic after a short delay if the enemy stops moving after knockback
        Invoke(nameof(ResetRigidbody), 0.3f);  // Adjust time as needed
    }

    // This will reset the Rigidbody back to kinematic after knockback
    private void ResetRigidbody()
    {
        
        if (health <= 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
        rb.linearVelocity = Vector2.zero; // Stop any remaining velocity
        State = EnemyStates.IDLE;
        StartCoroutine(Wander());
        hitbox.enabled = true;
        //rb.bodyType = RigidbodyType2D.Kinematic;  // Switch back to kinematic
    }

    private IEnumerator Wander()
    {
        while (true)
        {

            State = EnemyStates.MOVE;
            direction = GetRandomDirection();
            SetFacingDirection(direction);

            float moveTime = Random.Range(minMoveTime, maxMoveTime);
            yield return new WaitForSeconds(moveTime);

            State = EnemyStates.IDLE;
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private Vector2 GetRandomDirection()
    {
        int dir = Random.Range(0, 4);
        switch (dir)
        {
            case 0:
                return Vector2.up;
            case 1:
                return Vector2.down;
            case 2:
                return Vector2.left;
            case 3:
                return Vector2.right;
            default:
                return Vector2.down;
        }
    }

    private void SetFacingDirection(Vector2 dir)
    {
        if (dir == Vector2.up)
        {
            Direction = FacingDir.UP;
        }
        else if (dir == Vector2.down)
        {
            Direction = FacingDir.DOWN;
        }
        else if (dir == Vector2.left)
        {
            Direction = FacingDir.LEFT;
        }
        else if (dir == Vector2.right)
        {
            Direction = FacingDir.RIGHT;
        }
    }

    public void TakeDamage(HurtBox hurtBox) // called from player when sword hits enemy
    {
        health -= hurtBox.damageValue;

        // Calculate the knockback direction and normalize it
        Vector2 knockbackDirection = (transform.position - hurtBox.transform.position).normalized;

        // Apply knockback in the normalized direction
        ApplyKnockback(knockbackDirection);

        anim.SetTrigger("Hurt");

        
    }

}
