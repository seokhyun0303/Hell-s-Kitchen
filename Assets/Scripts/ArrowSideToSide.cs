using UnityEngine;

public class ArrowSideToSide : MonoBehaviour
{
    public float moveSpeed = 1f; // Speed of the side-to-side motion
    public float moveDistance = 0.5f; // Distance to move left and right

    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position of the arrow
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new X position using a sine wave
        float newX = startPosition.x + Mathf.Sin(Time.time * moveSpeed) * moveDistance;

        // Apply the new position
        transform.position = new Vector3(newX, startPosition.y, startPosition.z);
    }
}
