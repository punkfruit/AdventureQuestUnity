using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public int damageValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HitBox"))
        {
            // Trigger the hit on the HitBox
            HitBox hitbox = other.GetComponent<HitBox>();
            if (hitbox != null)
            {
                hitbox.TakeDamage(this);
            }
        }

        //Debug.Log("testtttt");
    }
}
