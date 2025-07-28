using UnityEngine;

public class PropHealth : MonoBehaviour
{
    public int health = 1;
    public Animator anim;



    public void TakeDamage(HurtBox hurtBox)
    {
        health -= hurtBox.damageValue;

        if(health <= 0)
        {
            anim.SetTrigger("Destroy");
        }
    }


    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
