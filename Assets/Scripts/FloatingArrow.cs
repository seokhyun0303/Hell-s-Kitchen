using UnityEngine;

public class FloatingArrow : MonoBehaviour
{
    public float floatSpeed = 1f; // Speed of the up-and-down motion
    public float floatAmplitude = 0.5f; // Amplitude of the motion (how high/low it moves)
    private Vector3 startPosition; // Initial position of the arrow

    void Start()
    {
        // Store the starting position of the arrow
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new position using a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // Apply the new position
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
