using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHealth = 5;
    public float knockbackForce = 5f;
    public float stunTime = 0.3f;
    public GameObject deathEffect;
    public EnemyMotor motor; // assign
    public AudioClip damageSound;

    private int _health;

    void Awake() => _health = maxHealth;

    public void Hit(HurtBox hb) //assigned in inspector as a "dynamic" field, cant have more than 1 fields to pass in
    {
        _health -= hb.damageValue;
        AudioManager.instance.PlaySoundFXClip(damageSound, transform, 1f);
        if (_health <= 0) { Die(); return; }

        if (motor != null)
        {
            Vector2 kb = (transform.position - hb.transform.position).normalized * knockbackForce;
            motor.ApplyKnockback(kb, stunTime);
        }
        motor.anim?.SetTrigger("Hurt");
    }

    void Die()
    {
        if (deathEffect) Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
