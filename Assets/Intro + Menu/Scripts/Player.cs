using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour {
    //params
    [Header("Movement")]
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float jumpSpeed = 25f;
    [SerializeField] float climbSpeed = 2f;
    [SerializeField] Vector2 deathKick = new Vector2(15f, 15);
    [Header("Visual")]
    [SerializeField] Sprite[] spritePanier;
    //[SerializeField] Weapon weapon; //ARME 
    [SerializeField] GameObject panier;
    //state
    bool isAlive = true;

    //cached ref
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeet;
    SpriteRenderer myPanierSprite;
    float gravityScaleAtStart;
    float percentageCollected;
    [SerializeField] bool weaponUnlocked;

    

    //Message & methods

	void Start ()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
        myPanierSprite = GetComponentInChildren<SpriteRenderer>();
        //weapon = GetComponentInChildren<Weapon>(); //ARME
        weaponUnlocked = false;
	}
	

	void Update ()
    {
        if (!isAlive) { return; } 
        ClimbLadder();
        Run();
        Jump();
        FlipSprite();
        Death();
        Win();
        if (weaponUnlocked == true)
        {
           // Attack();
        }
	}

    private void Run()
    {
        float controlThrow = Input.GetAxis("Horizontal"); //-1 ; +1
        Vector2 playerVelocity = new Vector2(controlThrow * walkSpeed, myRigidBody.velocity.y);
        myRigidBody.AddForce(playerVelocity);
        //myRigidBody.velocity = playerVelocity;
        //bool playerIsMoving = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        //{
        //    myAnimator.SetBool("Running", playerIsMoving);
        //}
    }

    private void ClimbLadder()
    {
        if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = gravityScaleAtStart;
            return;
        }

        {
            myRigidBody.gravityScale = 0f;
            float controlClimb = Input.GetAxis("Vertical");
            Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x , controlClimb * climbSpeed);
            myRigidBody.velocity = climbVelocity;
            bool playerIsClimbing = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
            {
                myAnimator.SetBool("Climbing", playerIsClimbing);
            }
        }
    }

    private void Jump()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (Input.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            GetComponentInChildren<Transform>().localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1);
        }
    }

    //private void Attack() //ARME
    //{
    //    if (Input.GetButtonDown("Fire1"))
    //    {
    //        if (!weapon) { return; }
    //        else { StartCoroutine(weapon.ProcessHit()); myAnimator.SetTrigger("Attack"); }
    //    }
    //}

    private void Death()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            Destroy(panier);
            myAnimator.SetTrigger("Death");
            GetComponent<Rigidbody2D>().velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Collectibles")) && isAlive)
        {
            if (collision.GetComponent<Collectible>().GetWeapon() == true) { weaponUnlocked = true; }
            else { FindObjectOfType<GameSession>().AddToScore(10); UpdatePanier(); }
            collision.GetComponent<Collectible>().PlaySFX();
            Destroy(collision.gameObject);
        }
    }

    private void UpdatePanier()
    {
        percentageCollected = FindObjectOfType<GameSession>().GetPercentageProgress();
        if (percentageCollected <= 25 && percentageCollected > 0) { myPanierSprite.sprite = spritePanier[1]; walkSpeed *= 0.95f; }
        else if (percentageCollected <= 50 && percentageCollected > 25) { myPanierSprite.sprite = spritePanier[2]; /*walkSpeed *= 0.95f;*/ }
        else if (percentageCollected <= 75 && percentageCollected > 50) { myPanierSprite.sprite = spritePanier[3]; /*walkSpeed *= 0.95f;*/ }
        else if (percentageCollected <= 100 && percentageCollected > 75) { myPanierSprite.sprite = spritePanier[4]; /*walkSpeed *= 0.95f;*/ }
                }


    private void Win()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Interactables")))
        {
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}

 