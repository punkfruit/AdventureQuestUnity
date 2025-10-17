using System.Collections;
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
    public float speed = 50f;
    public bool canAttack = true;
    public PlayerStates playerState = PlayerStates.IDLE;
    public PlayerInput playerInput;
    public bool canMove = true;
    private bool prioritizeXAxis;
    public FacingDirection faceDirection = FacingDirection.UP;

    public WaitForSeconds stunTime = new WaitForSeconds(0.2f);

    // public int health, maxhealth;
    public float flash = 0f;
    public float knockbackForce = 10f;

    // --- internal caches ---
    private MaterialPropertyBlock _mpb;
    private Coroutine _saveCo;

    // Animator parameter hashes
    static readonly int MoveX = Animator.StringToHash("moveX");
    static readonly int MoveY = Animator.StringToHash("moveY");
    static readonly int LastMoveX = Animator.StringToHash("LastMoveX");
    static readonly int LastMoveY = Animator.StringToHash("LastMoveY");
    static readonly int Staff = Animator.StringToHash("Staff");
    static readonly int FlashTrg = Animator.StringToHash("Flash");
    static readonly int DeadBool = Animator.StringToHash("Dead");

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        _mpb = new MaterialPropertyBlock();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Hook into GameManager if present
        if (GameManager.instance != null)
        {
            GameManager.instance.player = this;
            playerInput = GameManager.instance.playerInput;
            if (playerInput != null)
            {
                playerInput.onControlsChanged += OnControlsChanged;
                OnControlsChanged(playerInput);
            }
        }

        // Camera target (guarded)
        if (CameraController.instance != null)
        {
            CameraController.instance.ChangeTarget(transform);
        }
    }

    private void OnEnable()
    {
        // Throttle position writes instead of every Update()
        _saveCo = StartCoroutine(SaveTick());
    }

    private void OnDisable()
    {
        if (_saveCo != null) StopCoroutine(_saveCo);
    }

    private void OnDestroy()
    {
        if (playerInput != null) playerInput.onControlsChanged -= OnControlsChanged;
        if (_saveCo != null) StopCoroutine(_saveCo);
        if (instance == this) instance = null;
    }

    private void OnControlsChanged(PlayerInput input)
    {
        // Available if you want to branch behavior/UI by scheme
        // string currentControlScheme = input.currentControlScheme;
        // string deviceType = currentControlScheme == "Gamepad" ? "Controller" : "Keyboard";
    }

    public void SetPlayerPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    private void FixedUpdate()
    {
        // Decide velocity
        if (playerState != PlayerStates.STUN)
        {
            Vector2 targetVel = (!IsBusy() && canMove) ? moveDirection * speed : Vector2.zero;
            theRB.linearVelocity = targetVel;

            if (playerState != PlayerStates.ATTACK)
            {
                playerState = (targetVel.sqrMagnitude > 0.0001f) ? PlayerStates.RUN : PlayerStates.IDLE;
            }
        }

        HandleAnimation();
    }

    private void Update()
    {
        // Use MPB instead of spr.material to avoid material instancing every frame
        if (spr != null)
        {
            spr.GetPropertyBlock(_mpb);
            _mpb.SetFloat("_BlendOpacity", flash);
            spr.SetPropertyBlock(_mpb);
        }
    }

    public void HandleAnimation()
    {
        if (anim == null) return;

        if (canMove)
        {
            if (prioritizeXAxis)
            {
                anim.SetFloat(MoveX, moveDirection.x);
                anim.SetFloat(MoveY, 0f);
            }
            else
            {
                anim.SetFloat(MoveX, 0f);
                anim.SetFloat(MoveY, moveDirection.y);
            }

            if (moveDirection != Vector2.zero && playerState != PlayerStates.ATTACK)
            {
                anim.SetFloat(LastMoveX, moveDirection.x);
                anim.SetFloat(LastMoveY, moveDirection.y);
                UpdateFacingDirection(); // based on movement
            }
        }
        else
        {
            anim.SetFloat(MoveX, 0f);
            anim.SetFloat(MoveY, 0f);
        }
    }

    public void SetMoveInput(Vector2 moveInput)
    {
        // Normalize to avoid diagonal speed boost if input isn't normalized upstream
        if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();

        // Priority resolution (ties favor X)
        prioritizeXAxis = Mathf.Abs(moveInput.x) >= Mathf.Abs(moveInput.y);

        moveDirection = canMove ? moveInput : Vector2.zero;
    }

    public void TryFire()
    {
        if (playerState != PlayerStates.ATTACK && canMove && canAttack && anim != null)
        {
            anim.SetTrigger(Staff);
            playerState = PlayerStates.ATTACK;
        }
    }

    public void ReturnToIdle()
    {
        // Hook this from animation event at end of attack
        if (playerState == PlayerStates.ATTACK)
            playerState = PlayerStates.IDLE;
    }

    public void TakeDamage(HurtBox hurtBox)
    {
        // Invincible during dialogue (guard singleton)
        if (DialogueManager.instance != null && DialogueManager.instance.dialogueIsPlaying)
            return;

        StopAllCoroutines(); // cancel knockback timer if any
        playerState = PlayerStates.STUN;
        theRB.linearVelocity = Vector2.zero;

        // Health check (guard singleton)
        if (HealthManager.instance != null && HealthManager.instance.TakeDamage(hurtBox.damageValue))
        {
            if (anim != null) anim.SetBool(DeadBool, true);
            canMove = false;
            return;
        }

        if (anim != null) anim.SetTrigger(FlashTrg);

        // Knockback
        Vector2 knockbackDirection = (transform.position - hurtBox.transform.position).normalized;
        theRB.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        // Recover after stun
        StartCoroutine(DisableMovementForKnockback());
    }

    private IEnumerator DisableMovementForKnockback()
    {
        canMove = false;
        yield return stunTime;
        theRB.linearVelocity = Vector2.zero;
        canMove = true;
        playerState = PlayerStates.IDLE;

        // Resume throttled saver
        if (_saveCo != null) StopCoroutine(_saveCo);
        _saveCo = StartCoroutine(SaveTick());
    }

    private bool IsBusy() => playerState == PlayerStates.ATTACK;

    // Method to dynamically update the facing direction
    private void UpdateFacingDirection()
    {
        if (moveDirection.x > 0f) faceDirection = FacingDirection.RIGHT;
        else if (moveDirection.x < 0f) faceDirection = FacingDirection.LEFT;
        else if (moveDirection.y > 0f) faceDirection = FacingDirection.UP;
        else if (moveDirection.y < 0f) faceDirection = FacingDirection.DOWN;
    }

    public void SetFacingDirection(FacingDirection facingDirection)
    {
        faceDirection = facingDirection;
    }

    // Throttled save writer
    private IEnumerator SaveTick()
    {
        var wait = new WaitForSeconds(0.5f);
        while (true)
        {
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.CurrentData.playerPosition =
                    new SaveManager.Vector3Data(transform.position);
            }
            yield return wait;
        }
    }
}
