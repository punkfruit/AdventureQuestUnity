using UnityEngine;

public interface IMoveProvider
{
    // Return desired velocity in world space this frame (Brain supplies this).
    Vector2 GetDesiredVelocity();
}

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMotor : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Animator anim;
    public Rigidbody2D rb { get; private set; }

    private IMoveProvider _provider;
    private Vector2 _overrideVelocity;
    private float _overrideTimer;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    public void SetProvider(IMoveProvider provider) => _provider = provider;

    public void ApplyKnockback(Vector2 impulse, float stunTime)
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(impulse, ForceMode2D.Impulse);
        _overrideTimer = stunTime;
        _overrideVelocity = Vector2.zero; // lock out brain control during stun
    }

    void Update()
    {
        var v = rb.linearVelocity;
        anim?.SetFloat("dirX", v.x);
        anim?.SetFloat("dirY", v.y);
        //anim?.SetFloat("speed", v.sqrMagnitude);
    }

    void FixedUpdate()
    {
        if (_overrideTimer > 0f)
        {
            _overrideTimer -= Time.fixedDeltaTime;
            // Let physics settle; we don’t add brain velocity while stunned.
            return;
        }

        Vector2 want = _provider != null ? _provider.GetDesiredVelocity() : Vector2.zero;
        rb.linearVelocity = want * moveSpeed;
    }
}
