using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum walkDirection { North, South, West, East }
public enum charClass { Blank, Wizard, Rogue, Barbarian, Bard }
public enum weaponTypes { None, WizardStaff, Sword } //ill add more later
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("movement")]
    public walkDirection walkdir;
    public charClass classs;
    public SpriteRenderer headSPR, bodySPR;
    //public Vector2 debugtest;
    public Sprite[] heads;
    public Sprite[] body;
    private Vector2 input;

    public float moveSpeed;
    public float spriteSwitchThreshhold = 0.5f;
    public float spriteSwitchThreshhold2 = 0.4f;
    public Rigidbody2D theRB;
    public Animator anim;
    public bool canMove = true;

    [Header("combat")]
    public weaponTypes currentWeapon;
    public Transform weaponSpawnPoint; //where the weapon will spawn when user attacks
    public Transform wEast, wWest, wNorth, wSouth; //these feed into weaponSpawnPoint
    public GameObject sword; //the actual weapon, its called sword but it can be any of the ones defined in the enum weaponTypes
    public GameObject[] weapons;
    public bool canSwing = true;

    [Header("Mobile Weapon")] //the 'fake' weapon model that hovers around the player!
    public GameObject mobileStaff;
    public SpriteRenderer mobileStaffSPR;
    public Transform[] wizStaff, swordT;
    public Sprite[] mobileStaffSprites;

    [Header("Health")]
    public int health;
    public int maxHealth;


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

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        HealthManager.instance.HeartUpdate(health);
    }



    private void FixedUpdate()
    {
        
        if (canMove)
        {
            theRB.velocity = input * moveSpeed;


            if (input != Vector2.zero)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }


           

        }
        else
        {
            theRB.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }


        float horizontalVelocity = Mathf.Abs(theRB.velocity.x);
        float verticalVelocity = Mathf.Abs(theRB.velocity.y);

        if (horizontalVelocity > verticalVelocity && horizontalVelocity > spriteSwitchThreshhold)
        {
            walkdir = theRB.velocity.x > 0 ? walkDirection.East : walkDirection.West;
        }
        else if (verticalVelocity > horizontalVelocity && verticalVelocity > spriteSwitchThreshhold)
        {
            walkdir = theRB.velocity.y > 0 ? walkDirection.North : walkDirection.South;
        }



        WalkDirSpriteSwitch();

        if(currentWeapon == weaponTypes.None)
        {
            mobileStaff.SetActive(false);
        }
    }


    public void WalkDirSpriteSwitch() //this will change many of the paramaters depending on which way the player is facing, what their class is, and what weapon theyre using. im doing it this way cause its important for the player to change things on the fly, like the current class
    {
        switch (walkdir)
        {
            case walkDirection.West:
                weaponSpawnPoint = wWest;
                
                
                switch (classs)
                {
                    case charClass.Blank:
                        headSPR.sprite = heads[12];
                        bodySPR.sprite = body[0];
                        break;
                    case charClass.Wizard:
                        headSPR.sprite = heads[0];
                        bodySPR.sprite = body[0];
                        headSPR.flipX = true;
                        break;
                    case charClass.Rogue:
                        headSPR.sprite = heads[6];
                        bodySPR.sprite = body[3];
                        headSPR.flipX = true;
                        bodySPR.flipX = true;
                        break;
                    case charClass.Barbarian:
                        headSPR.sprite = heads[3];
                        bodySPR.sprite = body[1];
                        headSPR.flipX = true;
                        bodySPR.flipX = true;
                        break;
                    case charClass.Bard:
                        headSPR.sprite = heads[9];
                        bodySPR.sprite = body[0];
                        headSPR.flipX = true;
                        break;
                }

                switch (currentWeapon)
                {
                    case weaponTypes.None:
                        sword = weapons[0];
                        break;
                    case weaponTypes.WizardStaff:
                        sword = weapons[1];
                        mobileStaffSPR.sprite = mobileStaffSprites[1];
                        mobileStaff.transform.position = wizStaff[0].position;
                        mobileStaff.transform.rotation = wizStaff[0].rotation;
                        mobileStaff.transform.localScale = wizStaff[0].localScale;
                        mobileStaffSPR.sortingOrder = 5;
                        break;
                    case weaponTypes.Sword:
                        sword = weapons[3];
                        mobileStaffSPR.sprite = mobileStaffSprites[2];
                        mobileStaff.transform.position = swordT[0].position;
                        mobileStaff.transform.rotation = swordT[0].rotation;
                        mobileStaff.transform.localScale = swordT[0].localScale;
                        mobileStaffSPR.sortingOrder = 5;
                        break;
                }

                break;

                
            case walkDirection.East:
                weaponSpawnPoint = wEast;
                
                switch (classs)
                {
                    case charClass.Blank:
                        headSPR.sprite = heads[12];
                        bodySPR.sprite = body[0];
                        break;
                    case charClass.Wizard:
                        headSPR.sprite = heads[0];
                        bodySPR.sprite = body[0];
                        headSPR.flipX = false;
                        break;
                    case charClass.Rogue:
                        headSPR.sprite = heads[6];
                        bodySPR.sprite = body[3];
                        headSPR.flipX = false;
                        bodySPR.flipX = false;
                        break;
                    case charClass.Barbarian:
                        headSPR.sprite = heads[3];
                        bodySPR.sprite = body[1];
                        headSPR.flipX = false;
                        bodySPR.flipX = false;
                        break;
                    case charClass.Bard:
                        headSPR.sprite = heads[9];
                        bodySPR.sprite = body[0];
                        headSPR.flipX = false;
                        break;
                }

                switch (currentWeapon)
                {
                    case weaponTypes.None:
                        sword = weapons[0];
                        break;
                    case weaponTypes.WizardStaff:
                        sword = weapons[1];
                        mobileStaffSPR.sprite = mobileStaffSprites[1];
                        mobileStaff.transform.position = wizStaff[1].position;
                        mobileStaff.transform.rotation = wizStaff[1].rotation;
                        mobileStaff.transform.localScale = wizStaff[1].localScale;
                        mobileStaffSPR.sortingOrder = 5;
                        break;
                    case weaponTypes.Sword:
                        sword = weapons[3];
                        mobileStaffSPR.sprite = mobileStaffSprites[2];
                        mobileStaff.transform.position = swordT[1].position;
                        mobileStaff.transform.rotation = swordT[1].rotation;
                        mobileStaff.transform.localScale = swordT[1].localScale;
                        mobileStaffSPR.sortingOrder = 5;
                        break;
                }

                break;
            case walkDirection.North:
                weaponSpawnPoint = wNorth;
                switch (classs)
                {
                    case charClass.Blank:
                        headSPR.sprite = heads[12];
                        bodySPR.sprite = body[0];
                        break;
                    case charClass.Wizard:
                        headSPR.sprite = heads[1];
                        bodySPR.sprite = body[0];
                        break;
                    case charClass.Rogue:
                        headSPR.sprite = heads[7];
                        bodySPR.sprite = body[0];
                        break;
                    case charClass.Barbarian:
                        headSPR.sprite = heads[4];
                        bodySPR.sprite = body[0];
                        break;
                    case charClass.Bard:
                        headSPR.sprite = heads[10];
                        bodySPR.sprite = body[0];
                        break;
                }

                switch (currentWeapon)
                {
                    case weaponTypes.None:
                        sword = weapons[0];
                        break;
                    case weaponTypes.WizardStaff:
                        sword = weapons[2];
                        mobileStaffSPR.sprite = mobileStaffSprites[1];
                        mobileStaff.transform.position = wizStaff[2].position;
                        mobileStaff.transform.rotation = wizStaff[2].rotation;
                        mobileStaff.transform.localScale = wizStaff[2].localScale;
                        mobileStaffSPR.sortingOrder = -5;
                        break;
                    case weaponTypes.Sword:
                        sword = weapons[4];
                        mobileStaffSPR.sprite = mobileStaffSprites[2];
                        mobileStaff.transform.position = swordT[2].position;
                        mobileStaff.transform.rotation = swordT[2].rotation;
                        mobileStaff.transform.localScale = swordT[2].localScale;
                        mobileStaffSPR.sortingOrder = -5;
                        break;
                }

                break;
            case walkDirection.South:
                weaponSpawnPoint = wSouth;
                switch (classs)
                {
                    case charClass.Blank:
                        headSPR.sprite = heads[12];
                        bodySPR.sprite = body[0];
                        break;
                    case charClass.Wizard:
                        headSPR.sprite = heads[2];
                        bodySPR.sprite = body[0];
                        break;
                    case charClass.Rogue:
                        headSPR.sprite = heads[8];
                        bodySPR.sprite = body[4];
                        break;
                    case charClass.Barbarian:
                        headSPR.sprite = heads[5];
                        bodySPR.sprite = body[2];
                        break;
                    case charClass.Bard:
                        headSPR.sprite = heads[11];
                        bodySPR.sprite = body[0];
                        break;
                }

                switch (currentWeapon)
                {
                    case weaponTypes.None:
                        sword = weapons[0];
                        break;
                    case weaponTypes.WizardStaff:
                        sword = weapons[2];
                        mobileStaffSPR.sprite = mobileStaffSprites[1];
                        mobileStaff.transform.position = wizStaff[3].position;
                        mobileStaff.transform.rotation = wizStaff[3].rotation;
                        mobileStaff.transform.localScale = wizStaff[3].localScale;
                        mobileStaffSPR.sortingOrder = 5;
                        break;
                    case weaponTypes.Sword:
                        sword = weapons[4];
                        mobileStaffSPR.sprite = mobileStaffSprites[2];
                        mobileStaff.transform.position = swordT[3].position;
                        mobileStaff.transform.rotation = swordT[3].rotation;
                        mobileStaff.transform.localScale = swordT[3].localScale;
                        mobileStaffSPR.sortingOrder = 7;
                        break;
                }

                break;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    public void Swing(InputAction.CallbackContext context)
    {
        if (context.performed && currentWeapon != weaponTypes.None)
        {
            if (weaponSpawnPoint != null && canSwing)
            {

                if (canMove)
                {
                    //var swrd = Instantiate(sword, weaponSpawnPoint);
                    Vector3 poop = new Vector3(weaponSpawnPoint.position.x, weaponSpawnPoint.position.y, weaponSpawnPoint.position.z);
                    var swrd = Instantiate(sword, poop, weaponSpawnPoint.rotation);

                    swrd.transform.parent = gameObject.transform;
                    swrd.transform.localScale = weaponSpawnPoint.localScale;
                    canSwing = false;
                    canMove = false;

                    mobileStaff.SetActive(false);
                    //Debug.Log("swung");
                }

            }
        }
    }


    public void TakeDamage(int dam)
    {
        health -= dam;
        HealthManager.instance.HeartUpdate(health);
        if(health <= 0)
        {
            //die. ill add this later
        }
    }
}
