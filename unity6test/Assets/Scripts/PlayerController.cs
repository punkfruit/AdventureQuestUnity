using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerStates { IDLE, RUN, ATTACK, STUN }
public enum FacingDirection { LEFT, RIGHT, UP, DOWN }

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Technical")]
    public Animator anim;
    public Rigidbody2D theRB;
    public SpriteRenderer spr;
    public Vector2 moveDirection;
    public float speed = 50;
    public bool canAttack = true;
    public PlayerStates playerState = PlayerStates.IDLE;
    public PlayerInput playerInput;
    public bool canMove = true;
    private bool prioritizeXAxis;
    public FacingDirection faceDirection = FacingDirection.UP;

    public WaitForSeconds stunTime = new WaitForSeconds(0.2f);

    //public int health, maxhealth;
    public float flash = 0;
    public float knockbackForce = 10f; // Adjust this value as needed


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

        if (GameManager.instance != null)
        {
            GameManager.instance.player = this;
        }

        CameraController.instance.ChangeTarget(transform);
        playerInput = GameManager.instance.playerInput;
        DontDestroyOnLoad(this);
       // health = maxhealth;
        playerInput.onControlsChanged += OnControlsChanged;
        OnControlsChanged(playerInput);
    }

    private void OnControlsChanged(PlayerInput input)
    {
        string currentControlScheme = input.currentControlScheme;
        string deviceType = currentControlScheme == "Gamepad" ? "Controller" : "Keyboard";
    }

    public void SetPlayerPosition(Vector3 pos)
    {
        transform.position = pos;
     //return new Vector3(transform.position.x,transform.position.y,0);
    }

    private void FixedUpdate()
    {
        if(playerState != PlayerStates.STUN)
        {
            if (playerState != PlayerStates.ATTACK && canMove)
            {
                theRB.linearVelocity = moveDirection * speed;
            }
            else
            {
                theRB.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            //moveDirection = Vector2.zero;
        }
        

        HandleAnimation();
    }

    private void Update()
    {
        spr.material.SetFloat("_BlendOpacity", flash);
        //SaveManager.Instance.CurrentData.playerPosition = transform.position;
        SaveManager.Instance.CurrentData.playerPosition = new SaveManager.Vector3Data(transform.position);

    }

    public void HandleAnimation()
    {
        if (canMove)
        {
            if (prioritizeXAxis)
            {
                anim.SetFloat("moveX", moveDirection.x);
                anim.SetFloat("moveY", 0);
            }
            else
            {
                anim.SetFloat("moveX", 0);
                anim.SetFloat("moveY", moveDirection.y);
            }

            if (moveDirection != Vector2.zero && playerState != PlayerStates.ATTACK)
            {
                anim.SetFloat("LastMoveX", moveDirection.x);
                anim.SetFloat("LastMoveY", moveDirection.y);

                UpdateFacingDirection(); // Update facing direction based on movement
            }
        }
        else
        {
            anim.SetFloat("moveX", 0);
            anim.SetFloat("moveY", 0);

            //anim.SetFloat("LastMoveX", 0);
            //anim.SetFloat("LastMoveY", 0);
        }
        
    }

    public void SetMoveInput(Vector2 moveInput)
    {
        if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
        {
            prioritizeXAxis = true;
        }
        else if (Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x))
        {
            prioritizeXAxis = false;
        }

        moveDirection = canMove ? moveInput : Vector2.zero;
    }

    public void TryFire()
    {
        if (playerState != PlayerStates.ATTACK && canMove && canAttack)
        {
            anim.SetTrigger("Staff");
            playerState = PlayerStates.ATTACK;
        }
    }

    public void ReturnToIdle()
    {
        playerState = PlayerStates.IDLE;
    }

    public void TakeDamage(HurtBox hurtBox)
    {
        if(!DialogueManager.instance.dialogueIsPlaying)//if theres dialogue playing the player is invincible! lol!
        {
            StopAllCoroutines();
            playerState = PlayerStates.STUN;
            theRB.linearVelocity = Vector2.zero;

            // Reduce health
            if (HealthManager.instance.TakeDamage(hurtBox.damageValue))
            {
                anim.SetBool("Dead", true);
                canMove = false;
                canMove = false;
                return;
            }
            anim.SetTrigger("Flash"); // Flash player character red

            // Calculate knockback direction
            Vector2 knockbackDirection = (transform.position - hurtBox.transform.position).normalized;

            // Apply knockback force
            theRB.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);


            // Optional: temporarily disable player movement during knockback
            StartCoroutine(DisableMovementForKnockback());
        }
        
    }

    private IEnumerator DisableMovementForKnockback()
    {
        canMove = false;
        yield return stunTime; // Adjust the duration of the knockback effect
        theRB.linearVelocity = Vector3.zero;
        canMove = true;
        playerState = PlayerStates.IDLE;
    }

    // Method to dynamically update the facing direction
    private void UpdateFacingDirection()
    {
        if (moveDirection.x > 0)
        {
            faceDirection = FacingDirection.RIGHT;
        }
        else if (moveDirection.x < 0)
        {
            faceDirection = FacingDirection.LEFT;
        }
        else if (moveDirection.y > 0)
        {
            faceDirection = FacingDirection.UP;
        }
        else if (moveDirection.y < 0)
        {
            faceDirection = FacingDirection.DOWN;
        }
    }

    public void SetFacingDirection(FacingDirection facingDirection)
    {
        faceDirection = facingDirection;
    }
}
