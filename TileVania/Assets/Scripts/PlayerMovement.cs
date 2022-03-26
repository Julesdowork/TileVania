using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float climbSpeed;
    float startingGravity = 8f;

    Rigidbody2D myRigidbody;
    Animator animator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myRigidbody.gravityScale = startingGravity;
        animator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            return;
        
        if (value.isPressed)
        {
            // do stuff
            myRigidbody.velocity += new Vector2(0, jumpSpeed);
        }
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        if (Mathf.Abs(myRigidbody.velocity.x) > 0)
            animator.SetBool("IsRunning", true);
        else
            animator.SetBool("IsRunning", false);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
    }

    private void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbable")))
        {
            myRigidbody.gravityScale = startingGravity;
            animator.SetBool("IsClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        animator.SetBool("IsClimbing", playerHasVerticalSpeed);
    }
}
