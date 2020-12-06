using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    [SerializeField] float moveSpeed = 3f;

    bool isAlive;

    Rigidbody2D myRigidBody;
    CapsuleCollider2D myBody;
    BoxCollider2D myFeet;
    Animator myAnimator;



    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        isAlive = true;
    }


    void Update()
    {
        Move();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isAlive == true)
        {
            if (moveSpeed < 0) { moveSpeed = +Mathf.Abs(moveSpeed); }
            else { moveSpeed = -Mathf.Abs(moveSpeed); }
            GetComponentInChildren<Transform>().localScale = new Vector2(-Mathf.Sign(myRigidBody.velocity.x), 1);
        }
    }

    public void OnHitWeapon()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        isAlive = false;
        myAnimator.SetTrigger("Death");
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private void Move()
    {
        if (isAlive == true)
        {
            myRigidBody.velocity = new Vector2(moveSpeed, 0);
        }
        else { myRigidBody.velocity = Vector2.zero; }
    }

}
