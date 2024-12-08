using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    [System.Serializable]
    public class Objective
    {
        public string description; // Full description of the objective
        public TextMeshProUGUI textElement; // Associated UI Text element
        public GameObject interactableObject; // In-world object linked to this objective
        public float interactionDistance = 3f; // Distance for interaction
        public GameObject interactionOverlay; // UI overlay for "E to interact"
        public GameObject requiredObject; // Required object for completing this objective
        public bool isCompleted = false; // Tracks if the objective is completed
    }

    public Objective[] objectives; // List of objectives
    public Transform player; // Player transform reference
    public KeyCode interactionKey = KeyCode.E; // Key to interact with objectives

    void Update()
    {
        for (int i = 0; i < objectives.Length; i++)
        {
            Objective objective = objectives[i];

            // Skip completed objectives or those without interactable objects
            if (objective.isCompleted || objective.interactableObject == null) continue;

            // Ensure the previous objective is completed before allowing interaction
            if (i > 0 && !objectives[i - 1].isCompleted)
            {
                Debug.LogWarning($"Objective {i} is locked. Complete Objective {i - 1} first.");
                if (objective.interactionOverlay != null)
                    objective.interactionOverlay.SetActive(false);
                continue;
            }

            // Calculate distance between player and interactable object
            float distance = Vector3.Distance(player.position, objective.interactableObject.transform.position);

            if (distance <= objective.interactionDistance)
            {
                // Show the interaction overlay
                if (objective.interactionOverlay != null)
                    objective.interactionOverlay.SetActive(true);

                // Check for interaction input
                if (Input.GetKeyDown(interactionKey))
                {
                    if (objective.requiredObject != null) // Check if the objective has a specific required object
                    {
                        if (PlayerIsCarryingObject(objective.requiredObject))
                        {
                            CompleteObjective(i); // Complete the objective if the player has the required object
                        }
                        else
                        {
                            Debug.LogWarning($"Objective {i} requirements not met: Player is not carrying the required object ({objective.requiredObject.name}).");
                        }
                    }
                    else
                    {
                        CompleteObjective(i); // Complete the objective if no required object is specified
                    }
                }
            }
            else
            {
                // Hide interaction overlay when out of range
                if (objective.interactionOverlay != null)
                    objective.interactionOverlay.SetActive(false);
            }
        }
    }

    public void CompleteObjective(int index)
    {
        if (index < 0 || index >= objectives.Length) return;

        Objective objective = objectives[index];
        if (objective.isCompleted) return; // Skip if already completed

        objective.isCompleted = true; // Mark as completed
        Debug.Log($"Objective {index} completed: {objective.description}");

        // Update UI text with a strike-through while keeping the original description intact
        if (objective.textElement != null)
        {
            objective.textElement.text = $"<s>{objective.textElement.text}</s>"; // Apply strike-through
            objective.textElement.color = Color.gray; // Change color to gray
        }

        // Keep the interactable object active but adjust its behavior
        if (objective.interactableObject != null)
        {
            Collider collider = objective.interactableObject.GetComponent<Collider>();
            if (collider != null) collider.enabled = true; // Ensure collider remains active

            Debug.Log($"Objective {objective.description} remains visible and functional.");
        }

        // Hide the interaction overlay
        if (objective.interactionOverlay != null)
            objective.interactionOverlay.SetActive(false);
    }

    private bool PlayerIsCarryingObject(GameObject requiredObject)
    {
        // Get the InteractionController component on the player
        InteractionController interactionController = player.GetComponent<InteractionController>();
        if (interactionController != null && interactionController.currentlyHeldObject == requiredObject)
        {
            Debug.Log($"Player is carrying the required object: {requiredObject.name}");
            return true;
        }

        Debug.Log($"Player is NOT carrying the required object: {requiredObject.name}");
        return false;
    }
}
