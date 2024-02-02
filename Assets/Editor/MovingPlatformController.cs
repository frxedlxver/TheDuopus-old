using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    public PlatformPath Path;
    public GameObject Platform;

    public float MoveSpeed;
    public float RotationSpeed;

    private Transform curPoint;
    private Transform nextPoint;
    private bool isRotating;
    private Rigidbody2D platformRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        curPoint = Path.pathPoints[0]; // Assuming pathPoints is an array or list of points
        nextPoint = Path.GetNextPoint(curPoint);
        isRotating = true;
        platformRigidbody = Platform.GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    // FixedUpdate is called every fixed framerate frame
    void FixedUpdate()
    {
        // Check if the platform needs to rotate
        if (isRotating)
        {
            RotateTowardsNextPoint();
        }
        else
        {
            MoveTowardsNextPoint();
        }
    }

    private void RotateTowardsNextPoint()
    {
        Vector2 directionToNextPoint = (nextPoint.position - Platform.transform.position).normalized;
        float angle = Mathf.Atan2(directionToNextPoint.y, directionToNextPoint.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Rotate the platform
        Platform.transform.rotation = Quaternion.RotateTowards(Platform.transform.rotation, targetRotation, RotationSpeed * Time.fixedDeltaTime);

        // Check if the platform is facing the next point
        if (Quaternion.Angle(Platform.transform.rotation, targetRotation) < 1f) // Threshold for rotation completion
        {
            isRotating = false;
        }
    }

    private void MoveTowardsNextPoint()
    {
        Vector2 newPosition = Vector2.MoveTowards(platformRigidbody.position, nextPoint.position, MoveSpeed * Time.fixedDeltaTime);
        platformRigidbody.MovePosition(newPosition);

        // Check if the platform has reached the next point
        if (Vector2.Distance(platformRigidbody.position, nextPoint.position) < 0.01f)
        {
            curPoint = nextPoint;
            nextPoint = Path.GetNextPoint(curPoint);
            isRotating = true; // Start rotating towards the new next point
        }
    }
}
