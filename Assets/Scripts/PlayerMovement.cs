using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement
    private float horizontal;
    private float speed = 10f;
    private float jumpingPower = 14f;
    private bool isFacingRight = true;

    private bool doubleJump;

    private float vertical;
    private float Climbspeed = 8f;
    [SerializeField] private bool isLadder;
    public bool isClimbing;

    private Animator anim;

    //Animation States
    private enum MovementState { Idle, Running, Jumping, Falling, Climbing }

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        #region General Movement
        //Movement
        horizontal = Input.GetAxisRaw("Horizontal");

       //Jumping
        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                doubleJump = !doubleJump;
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        #endregion


        //Flips The Sprite
        Flip();

        UpdateAnimationState();
    }

    #region Climbing Code
    //Climbing
    private void FixedUpdate()
    {

        vertical = Input.GetAxis("Vertical");

        if (isLadder && Mathf.Abs(vertical) >= 0.1f)
        {
            isClimbing = true;
        }

        if (isClimbing)
        {
            rb.gravityScale = 8f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * Climbspeed);
        }
        else
        {
            rb.gravityScale = 4f;
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
    #endregion

    //GroundCheck
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    //Flips Sprite
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {

            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        }
    }

    #region Animations
    //Decides When To Play The Right Animation
    private void UpdateAnimationState()
    {
        MovementState state;
        
        if (horizontal > .1f)
        {
            state = MovementState.Running;
        }
        else if (horizontal < -.1f)
        {
            state = MovementState.Running;
        }
        else
        {
            state = MovementState.Idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.Jumping;
        }

        if (rb.velocity.y < -.1f)
        {
            state = MovementState.Falling;
        }

        if (isClimbing)
        {
            state = MovementState.Climbing;
        }

        anim.SetInteger("state", (int)state);
    }
    #endregion
}

