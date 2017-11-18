using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CnControls;

public class PlayerControllerScripts : MonoBehaviour
{

    public float maxSpeed = 8f;
    bool facingRight = true;

    Animator anim;

    bool grounded = false;
    public Transform groundCheckRight;
    public Transform groundCheckLeft;
    float groundRadius = 0.03f;
    public LayerMask whatIsGround;
    public float jumpForce = 1400f;

    private Rigidbody2D rigidBody2D;

    public TextMesh textForDebug;

    public int stompDamage = 60;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();

    }

    

    // Update is called once per frame
    void Update()
    {
        if (grounded && (Input.GetKeyDown(KeyCode.C) || CnInputManager.GetButtonDown("Jump")))
        {

            textForDebug.text = "touchCount" + Input.touchCount;
            anim.SetBool("Ground", false);
            rigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
            //rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, rigidBody2D.velocity.y - jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.R) || CnInputManager.GetButtonDown("ChangeWeapon"))
        {
            anim.SetTrigger("ChangeWeapon");

            int curWeaponState = anim.GetInteger("WeaponState");

            if (curWeaponState == 0)
            {
                anim.SetInteger("WeaponState", 1);
                curWeaponState = 1;
            }
            else if (curWeaponState == 1)
            {
                anim.SetInteger("WeaponState", 0);
                curWeaponState = 0;
            }
        }
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //   
        //    anim.SetTrigger("ChangeWeapon");
        //    anim.SetInteger("WeaponState", 0);
        //}

        

    }

    void FixedUpdate()
    {
        //grounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.003f, 0.003f), whatIsGround);
        //grounded = Physics2D.OverlapCircle(groundCheckRight.position, groundRadius, whatIsGround);
        //if(!grounded)
        //    Physics2D.OverlapCircle(groundCheckLeft.position, groundRadius, whatIsGround);

        grounded = Physics2D.OverlapCircle(groundCheckRight.position, groundRadius, whatIsGround)
            || Physics2D.OverlapCircle(groundCheckLeft.position, groundRadius, whatIsGround);

        Debug.Log("Right Collision: " + Physics2D.OverlapCircle(groundCheckRight.position, groundRadius, whatIsGround));
        Debug.Log("Left Collision: " + Physics2D.OverlapCircle(groundCheckLeft.position, groundRadius, whatIsGround));


        anim.SetBool("Ground", grounded);



        float move = CnInputManager.GetAxis("Horizontal");

        rigidBody2D.velocity = new Vector2(move * maxSpeed, rigidBody2D.velocity.y);

        anim.SetFloat("Speed", Mathf.Abs(move));
        anim.SetFloat("vSpeed", rigidBody2D.velocity.y);

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.tag == "Enemy")
        {
            //if ((groundCheckRight.position.y > collision.transform.Find("JumpDamagedThreshold").position.y)
            if ((groundCheckRight.position.y > collision.GetComponent<ChildObjectList>().getChildList()["centerPos"].position.y - 0.2f)
            && rigidBody2D.velocity.y <= 0)
            {
                Debug.Log("Collision! + " + collision);
                rigidBody2D.AddForce(Vector2.up * 15, ForceMode2D.Impulse);

                // Effect
                GameObject effectPrefab = Resources.Load("Prefabs/Effects/SparkleMuzzleFlashRed") as GameObject;

                GameObject MuzzleFlashRed = MonoBehaviour.Instantiate(effectPrefab) as GameObject;

                Vector3 particleSpawnPos = rigidBody2D.transform.position;
                particleSpawnPos.y -= 1f;

                MuzzleFlashRed.GetComponent<Transform>().position = particleSpawnPos;

                collision.GetComponent<EnemyUnitInfo>().deal_damage(stompDamage);

            }
        }
    }
    

    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    
    //}

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
