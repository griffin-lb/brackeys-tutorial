using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public float speed;
    public float jumpPower;
    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private bool wallSliding = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Flip player based on input
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // Set Animator parameters
        animator.SetFloat("yVelocity", body.linearVelocity.y);
        animator.SetBool("isGrounded", isGrounded());

        // Wall slide logic
        if (wallJumpCooldown > 0.2f)
        {
            // Regular jump
            if (Input.GetKey(KeyCode.Space) && isGrounded())
            {
                Jump();
            }

            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y); // Handle left & right movement

            // Wall sliding
            if (isWallSliding() && !isGrounded())
            {
                if (!wallSliding)
                {
                    wallSliding = true;
                    body.gravityScale = 0.2f; // Slow down gravity for wall sliding
                    body.linearVelocity = new Vector2(0, body.linearVelocity.y); // Stop horizontal movement
                    animator.SetBool("isWallSliding", true); // Set animation to wall slide
                    animator.SetBool("isRunning", false); // Ensure "isRunning" is false during wall slide
                }

                // Wall jump
                if (Input.GetKey(KeyCode.Space))
                {
                    wallJumpCooldown = 0; // Reset cooldown on wall jump
                    body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * speed * 1.5f, jumpPower); // Apply velocity for wall jump
                    wallSliding = false; // Stop wall sliding after jump
                    body.gravityScale = 2f; // Restore gravity
                    animator.SetTrigger("isWallJumping"); // Set wall jump animation
                    animator.SetBool("isWallSliding", false); // Stop wall slide animation
                }
            }
            else
            {
                if (wallSliding)
                {
                    wallSliding = false;
                    body.gravityScale = 2f;
                    animator.SetBool("isWallSliding", false); // Exit wall sliding if not sliding anymore
                }

                // Ensure player stops running animation when they are not moving
                if (Mathf.Abs(horizontalInput) > 0.01f && !isWallSliding())
                {
                    animator.SetBool("isRunning", true);
                }
                else
                {
                    animator.SetBool("isRunning", false);
                }
            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower); // Apply upward force for regular jump
        animator.SetTrigger("isJumping"); // Trigger jumping animation
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool isWallSliding()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null && horizontalInput != 0 && Mathf.Sign(horizontalInput) == Mathf.Sign(transform.localScale.x);
    }
}
