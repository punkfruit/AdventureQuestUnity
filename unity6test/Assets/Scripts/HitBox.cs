using UnityEngine;
using UnityEngine.Events;

public class HitBox : MonoBehaviour
{
    // UnityEvent that can pass an int (damage value)
    public UnityEvent<HurtBox> OnTakeDamage;

    public void TakeDamage(HurtBox hurtBox)
    {
        // Emit the signal here
        if (OnTakeDamage != null)
        {
            OnTakeDamage?.Invoke(hurtBox);
            //Debug.Log("HitBox took damage: " + damVal);
        }
    }
}
