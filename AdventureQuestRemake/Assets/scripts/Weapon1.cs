using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon1 : MonoBehaviour
{
    public int damage;
    public bool weapon = false;
    public bool hit = false;
    public SpriteRenderer spr;
    public Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        hit = false;
        if (weapon)
        {
            switch (PlayerController.instance.walkdir)
            {
                case walkDirection.North:
                    spr.sortingOrder = -5;
                    break;
                case walkDirection.East:
                    spr.sortingOrder = 5;
                    break;
                case walkDirection.South:
                    spr.sortingOrder = 5;
                    break;
                case walkDirection.West:
                    spr.sortingOrder = 5;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
        PlayerController.instance.canSwing = true;

        if (!DialogueManager.instance.dialogueIsPlaying)
        {
            PlayerController.instance.canMove = true;
            
        }

        PlayerController.instance.mobileStaff.SetActive(true);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (weapon)
        {
            if(other.tag == "Enemy")
            {
                if (!hit)
                {
                    other.GetComponent<EnemyController1>().DamageEnemy(damage);
                    hit = true;
                    Debug.Log("hit");
                }
            }

            if(other.tag == "Wall")
            {
                anim.SetTrigger("hitWall");
            }
        }
    }
}
