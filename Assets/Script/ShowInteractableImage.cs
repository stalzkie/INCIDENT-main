using UnityEngine;
using UnityEngine.UI;

public class ShowInteractableImage : MonoBehaviour
{
    public Transform player; // Assign the player's transform in the Inspector
    public Image interactImage; // The image to display when the object is interactable
    public float interactRange = 1f; // Distance within which the image will show
    public KeyCode interactionKey = KeyCode.E; // Key for interaction

    private bool isPlayerNear = false;
    private bool hasInteracted = false; // Track if the object has been interacted with

    void Start()
    {
        // Ensure the image is initially hidden
        if (interactImage != null)
        {
            interactImage.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (player == null || interactImage == null) return;

        // Check the distance between the player and this object
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactRange && !hasInteracted)
        {
            // Show the image when the player is in range
            if (!isPlayerNear)
            {
                isPlayerNear = true;
                interactImage.gameObject.SetActive(true);
            }

            // Check if the player presses the interaction key
            if (Input.GetKeyDown(interactionKey))
            {
                Interact();
            }
        }
        else
        {
            // Hide the image when the player is out of range
            if (isPlayerNear)
            {
                isPlayerNear = false;
                interactImage.gameObject.SetActive(false);
            }
        }
    }

    private void Interact()
    {
        Debug.Log("Player interacted with: " + gameObject.name);

        // Hide the interact image
        interactImage.gameObject.SetActive(false);

        // Mark the object as interacted
        hasInteracted = true;

        // Perform any additional logic for interaction here
    }
}
