using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 8f;
    public float sprintMultiplier = 1.5f; // Multiplier for sprint speed
    public float ascendSpeed = 5f;
    public float descendSpeed = 5f;
    public float underwaterGravity = -1f;
    public Transform cameraTransform;
    
    private Vector3 movement;
    private float verticalVelocity;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Get the camera's forward and right vectors
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Calculate movement vector using the full camera direction (including vertical)
        movement = (right * horizontal + forward * vertical).normalized;

        // Apply speed (with sprint check)
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speed * sprintMultiplier : speed;
        movement *= currentSpeed;

        // Handle additional vertical movement (swimming up/down)
        if (Input.GetButton("Jump"))
        {
            verticalVelocity = ascendSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            verticalVelocity = -descendSpeed;
        }
        else
        {
            // Apply underwater gravity when not actively swimming up/down
            verticalVelocity += underwaterGravity * Time.deltaTime;
            verticalVelocity = Mathf.Max(verticalVelocity, -descendSpeed);
        }

        // Add extra vertical velocity to the movement
        // Only add this if we're not already moving significantly in the vertical direction
        // This prevents doubling up on vertical movement when looking up/down
        if (Mathf.Abs(movement.y) < 0.1f)
        {
            movement.y += verticalVelocity;
        }

        // Move the player
        controller.Move(movement * Time.deltaTime);
    }
}