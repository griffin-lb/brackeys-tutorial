using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D body;
    private Animator animator;
    private bool isGrounded;
    

    private void Awake()    // Runs once when the scene is loaded
    {
        // Grab references for Rigidbody & Animator from game objects in Unity
        body = GetComponent<Rigidbody2D>(); // Check player object for Rigidbody component & store it inside the body variable
        animator = GetComponent<Animator>();
    }

    // Detect when player presses left or right and move body in that direction
    
    private void Update()   // Runs once every frame
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocityY);   // left & right movement

        // Flip player
        if(horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if(horizontalInput < -0.01f)
        transform.localScale = new UnityEngine.Vector3 (-1, 1, 1);

        // When space is pressed & player is grounded, maintain velocity on X axis & apply velocity of "speed" variable to the Y axis
        if(Input.GetKey(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // Set Animator parameters
        animator.SetBool("isRunning", horizontalInput != 0);    // As long as horizontal value is not 0 (standing still) isRunning = true
        animator.SetBool("isGrounded", isGrounded);
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocityX, speed);
        isGrounded = false;
    }

    // OnCollisionEnter2D = whenever a 2D object with a Rigidbody touches another 2D object with a Rigidbody
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;
    }
}