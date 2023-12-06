using UnityEngine;

public class DirectionalLineController : MonoBehaviour
{
    public LineRenderer directionLine;
    public Transform playerTransform;

    // Private field to store the original direction
    private Vector2 limitedDirection;

    // Public property with a getter
    public Vector2 Direction
    {
        get { return limitedDirection; }
    }

    void Update()
    {
        // Get the mouse position in world space
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the original direction from player to mouse
        Vector2 originalDirection = mousePosition - (Vector2)playerTransform.position;

        // Calculate the angle between the player and mouse
        float angle = Mathf.Atan2(originalDirection.y, originalDirection.x) * Mathf.Rad2Deg;

        // Limit the angle to a specific range (e.g., 90 degrees)
        float limitedAngle = Mathf.Clamp(angle, playerTransform.eulerAngles.z - 30f, playerTransform.eulerAngles.z + 60f);

        // Calculate the new direction after limiting the rotation
        limitedDirection = Quaternion.Euler(0, 0, limitedAngle) * Vector2.right;

        // Set the line renderer points
        directionLine.SetPosition(0, playerTransform.position);

        // Calculate the end point of the line based on the limited direction and original length
        Vector2 endPoint = (Vector2)playerTransform.position + limitedDirection.normalized * originalDirection.magnitude;

        directionLine.SetPosition(1, endPoint);
    }

}
