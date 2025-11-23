using UnityEngine;

public class OscillatingObstacle : MonoBehaviour
{
    [Header("Oscillation Settings")]
    public Vector3 rotationAxis = Vector3.up;   // Axis of oscillation
    public float angleLimit = 70f;              // From -angleLimit to +angleLimit
    public float frequency = 1f;                // Speed of oscillation

    private Quaternion initialRotation;

    void Start()
    {
        // Store starting rotation so oscillation is relative
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // Create oscillating angle from -limit to +limit
        float angle = Mathf.Sin(Time.time * frequency) * angleLimit;

        // Apply rotation relative to original rotation
        Quaternion oscillation = Quaternion.AngleAxis(angle, rotationAxis.normalized);
        transform.rotation = initialRotation * oscillation;
    }
}
