using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public float speed;
    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private bool wallSliding = false;
    

    private void Awake()    // Runs once when the scene is loaded
    {
        // Grab references for Rigidbody & Animator from game objects in Unity
        body = GetComponent<Rigidbody2D>(); // Check player object for Rigidbody component & store it inside the body variable
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Detect when player presses left or right and move body in that direction
    
    private void Update()   // Runs once every frame
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Flip player
        if(horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if(horizontalInput < -0.01f)
        transform.localScale = new UnityEngine.Vector3 (-1, 1, 1);

        // Set Animator parameters
        animator.SetFloat("yVelocity", body.linearVelocityY);
        animator.SetBool("isRunning", horizontalInput != 0);    // As long as horizontal value is not 0 (standing still) isRunning = true
        animator.SetBool("isGrounded", isGrounded());

        // Wall jump logic
        if (wallJumpCooldown < 0.2f)
        {
             // If Space is pressed & player is grounded, maintain velocity on X axis & apply velocity of "speed" variable to the Y axis
            if(Input.GetKey(KeyCode.Space) && isGrounded())
            {
                Jump();
            }

            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocityY);   // left & right movement

            if (isWallSliding() && !isGrounded())
            {
                if (!wallSliding)
                {
                    wallSliding = true;
                    body.gravityScale = 0.2f;
                    body.linearVelocity = Vector2.zero;
                    animator.SetBool("isWallSliding", true); // Start wall sliding
                }
            }
            else
            {
                if (wallSliding)
                {
                    wallSliding = false;
                    body.gravityScale = 1;
                    animator.SetBool("isWallSliding", false); // Exit wall sliding
                }
            }
        }
        else wallJumpCooldown += Time.deltaTime;
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocityX, speed);
        animator.SetTrigger("isJumping");
    }

    // OnCollisionEnter2D = whenever a 2D object with a Rigidbody touches another 2D object with a Rigidbody
    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool isWallSliding()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}