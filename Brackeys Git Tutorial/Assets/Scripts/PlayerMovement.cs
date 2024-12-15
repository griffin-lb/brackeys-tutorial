using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;

    private void Awake()    // Runs once when the scene is loaded
    {
        body = GetComponent<Rigidbody2D>(); // Check player object for Rigidbody component & store it inside the body variable
    }

    // Detect when player presses left or right and move body in that direction
    
    private void Update()   // Runs once every frame
    {
        body.linearVelocity = new Vector2(Input.GetAxis("Horizontal"), body.linearVelocityY);
    }
}
