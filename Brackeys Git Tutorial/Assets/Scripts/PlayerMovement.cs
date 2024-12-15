using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D body;
    

    private void Awake()    // Runs once when the scene is loaded
    {
        body = GetComponent<Rigidbody2D>(); // Check player object for Rigidbody component & store it inside the body variable
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

        // When space is pressed, maintain velocity on X axis & apply velocity of "speed" variable to the Y axis
        if(Input.GetKey(KeyCode.Space))
        {
            body.linearVelocity = new Vector2(body.linearVelocityX, speed);
        }
    }
}