using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum walkDirection { North, South, West, East }
public enum charClass { Blank, Wizard, Rogue, Barbarian, Bard }
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
    private Vector2 moveInput;
    public Rigidbody2D theRB;
    public Animator anim;
    public bool canMove = true;

    [Header("combat")]
    public Transform weaponSpawnPoint;
    public Transform wEast, wWest, wNorth, wSouth;
    public GameObject sword;
    public bool canSwing = true;

    private float offSetAmount = 0.01f;

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
    }

    // Update is called once per frame
    void Update()
    {

        /*
        if (canMove)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();

            theRB.velocity = moveInput * moveSpeed;


            if (moveInput != Vector2.zero)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }


            if (Input.GetButtonDown("Swing"))
            {
                if (weaponSpawnPoint != null && canSwing)
                {
                    var swrd = Instantiate(sword, weaponSpawnPoint);
                    swrd.transform.parent = gameObject.transform;
                    canSwing = false;
                }

            }

        }
        else
        {
            theRB.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }


        //debugtest = theRB.velocity;

        if (theRB.velocity.x > 0.1f)
        {
            walkdir = walkDirection.East;
        }
        else if (theRB.velocity.x < -0.1f)
        {
            walkdir = walkDirection.West;
        }

        if (theRB.velocity.y > 0.1f)
        {
            walkdir = walkDirection.North;
        }
        else if (theRB.velocity.y < -0.1f)
        {
            walkdir = walkDirection.South;
        }

        WalkDirSpriteSwitch();


        */
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


        //debugtest = theRB.velocity;

        if (theRB.velocity.x > 0.1f)
        {
            walkdir = walkDirection.East;
        }
        else if (theRB.velocity.x < -0.1f)
        {
            walkdir = walkDirection.West;
        }

        if (theRB.velocity.y > 0.1f)
        {
            walkdir = walkDirection.North;
        }
        else if (theRB.velocity.y < -0.1f)
        {
            walkdir = walkDirection.South;
        }

        WalkDirSpriteSwitch();
    }


    public void WalkDirSpriteSwitch()
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

                break;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    public void Swing(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (weaponSpawnPoint != null && canSwing)
            {
                //var swrd = Instantiate(sword, weaponSpawnPoint);
                Vector3 poop = new Vector3(weaponSpawnPoint.position.x, weaponSpawnPoint.position.y, weaponSpawnPoint.position.z);
                var swrd = Instantiate(sword, poop, weaponSpawnPoint.rotation);

                swrd.transform.parent = gameObject.transform;
                swrd.transform.localScale = weaponSpawnPoint.localScale;
                canSwing = false;

                //Debug.Log("swung");
            }
        }
    }
}
