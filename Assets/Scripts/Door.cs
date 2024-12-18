using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // If the player is coming from left (player's x position is less than door's x position), move camera to next room
            if (collision.transform.position.x < transform.position.x)
            {    
                cam.MoveToNewRoom(nextRoom);
            }
            // Otherwise move camera to previous room    
            else
            {
                cam.MoveToNewRoom(previousRoom);
            }
        }
    }
}
