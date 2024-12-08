using UnityEngine;
using UnityEngine.UI;

public class ObjectiveIndicator : MonoBehaviour
{
    public Transform player; // Assign the player's transform
    public Camera playerCamera; // Assign the player's camera
    public Image indicatorImage; // Assign the indicator image (UI only)
    public Transform objective; // The target objective
    public float maxScale = 1.5f; // Maximum scale (150% of normal size)
    public float interactionDistance = 2f; // Distance at which the objective is interactable
    public float maxOpacityDistance = 3f; // Maximum distance for full opacity

    private Vector3 originalScale;
    private bool isInteractable = false;
    private Rigidbody objectiveRigidbody; // Cache the Rigidbody for reference

    void Start()
    {
        // Store the original size of the indicator
        originalScale = indicatorImage.transform.localScale;

        // Ensure the indicator is visible initially
        indicatorImage.gameObject.SetActive(true);

        // Get and configure the Rigidbody
        objectiveRigidbody = objective.GetComponent<Rigidbody>();
        if (objectiveRigidbody != null)
        {
            objectiveRigidbody.isKinematic = false; // Allow physics interaction
        }
    }

    void Update()
    {
        if (objective == null || playerCamera == null || indicatorImage == null) return;

        // Calculate the distance between the player and the objective
        float distance = Vector3.Distance(player.position, objective.position);

        // Convert the objective's world position to screen position
        Vector3 screenPosition = playerCamera.WorldToScreenPoint(objective.position);

        // Check if the objective is in front of the camera
        if (screenPosition.z > 0)
        {
            // Position the indicator on the screen
            indicatorImage.transform.position = screenPosition;

            // Scale the indicator based on distance
            float scaleMultiplier = Mathf.Clamp(1 + (maxScale - 1) * (distance / maxOpacityDistance), 1f, maxScale);
            indicatorImage.transform.localScale = originalScale * scaleMultiplier;

            // Adjust opacity based on distance
            float opacity = Mathf.Clamp01(1 - (distance / maxOpacityDistance)); // Decreases opacity with distance
            Color currentColor = indicatorImage.color;
            indicatorImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, opacity);

            // Change color to yellow if the player is within interaction distance
            if (distance <= interactionDistance)
            {
                isInteractable = true;
                indicatorImage.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, opacity);

                // Check for interaction input (e.g., "E" key)
                if (Input.GetKeyDown(KeyCode.E))
                {
                    InteractWithObjective();
                }
            }
            else
            {
                isInteractable = false;
            }

            // Ensure the indicator is visible
            indicatorImage.enabled = true;
        }
        else
        {
            // Hide the indicator if the objective is behind the camera
            indicatorImage.enabled = false;
        }
    }

    private void InteractWithObjective()
    {
        Debug.Log("Objective interacted with!");

        // Hide the indicator
        indicatorImage.gameObject.SetActive(false);

        // Optional: Add interaction logic for the objective
    }
}
