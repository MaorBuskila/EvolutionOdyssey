using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public float smoothSpeed = 0.125f;  // How smoothly the camera follows the player
    public Vector3 offset;  // Offset from the player's position

    void FixedUpdate()
    {
        if (player == null)
            return;

        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Only change the x and y position of the camera
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}