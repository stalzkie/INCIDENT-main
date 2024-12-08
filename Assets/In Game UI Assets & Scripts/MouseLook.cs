using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
        // Lock the cursor to the game window
        Cursor.lockState = CursorLockMode.Locked;

        // Initialize xRotation to the camera's starting rotation
        xRotation = transform.localRotation.eulerAngles.x;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the player body horizontally (around the Y-axis)
        playerBody.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically (up and down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent camera flipping
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}