using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    public GameObject myHands;
    public Transform player; // Reference to the player transform
    public bool canpickup;
    private GameObject ObjectIwantToPickUp;
    private GameObject interactableObject;
    public bool hasItem;
    public float interactionRange = 5f;
    public Camera playerCamera;
    public Material highlightMaterial;
    private Material originalMaterial;
    private Material interactableoriginalMaterial;
    public Image interactImage;
    public GameObject jammedAlert;

    public bool isLookingAtDoor;
    private bool isDoorOpen;
    private GameObject doorObject;
    public GameObject currentlyHeldObject;

    public bool isLookingAtLever;
    private GameObject leverObject;

    public float doorOpenTime = 3f;

    // Keep track of doors that are currently "open"
    private Dictionary<GameObject, Coroutine> openDoors = new Dictionary<GameObject, Coroutine>();


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
        CheckForInteraction();

        if (canpickup || isLookingAtDoor || isLookingAtLever)
        {
            interactImage.gameObject.SetActive(true);
        }
        else
        {
            interactImage.gameObject.SetActive(false);
        }

        if (canpickup && Input.GetKeyDown("e") && !hasItem)
        {
            PickUp();
            interactImage.gameObject.SetActive(false);
        }

        if (hasItem && Input.GetKeyDown("g"))
        {
            Drop();
        }

        if (isLookingAtDoor && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoor();
            interactImage.gameObject.SetActive(false);
        }

        if (isLookingAtLever && Input.GetKeyDown(KeyCode.E))
        {
            ToggleLever();
            interactImage.gameObject.SetActive(false);
        }
    }

    void CheckForInteraction()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.green);
        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            if (hit.collider.gameObject.CompareTag("PickUp"))
            {
                HandlePickupObject(hit.collider.gameObject);
            }
            else if (hit.collider.gameObject.CompareTag("Door"))
            {
                HandleDoorObject(hit.collider.gameObject);
            }
            else if (hit.collider.gameObject.CompareTag("Lever"))
            {
                HandleLeverObject(hit.collider.gameObject);
            }
            else
            {
                ResetInteraction();
            }
        }
        else
        {
            ResetInteraction();
        }
    }

    void HandlePickupObject(GameObject pickupObject)
    {
        if (ObjectIwantToPickUp != pickupObject)
        {
            ResetHighlight();
            ObjectIwantToPickUp = pickupObject;
            originalMaterial = ObjectIwantToPickUp.GetComponent<Renderer>().material;
            ObjectIwantToPickUp.GetComponent<Renderer>().material = highlightMaterial;
        }

        canpickup = true;
        isLookingAtDoor = false;
        isLookingAtLever = false;
    }

    void HandleDoorObject(GameObject door)
    {
        ResetHighlight();
        interactableObject = door;
        interactableoriginalMaterial = interactableObject.GetComponent<Renderer>().material;
        interactableObject.GetComponent<Renderer>().material = highlightMaterial;
        isLookingAtDoor = true;
        doorObject = door;

        // Check if the door is jammed
        JammedDoor jammedDoor = door.GetComponent<JammedDoor>();
        if (jammedDoor != null)
        {
            // Highlight differently if the player isn't holding the required object
            if (currentlyHeldObject == null || currentlyHeldObject.name != jammedDoor.requiredObjectName)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(DisplayMessage());
                }
                Debug.Log("The door is jammed! You need to hold the correct object to open it.");
                canpickup = false;
                return;
            }

        }

        canpickup = false;
        isLookingAtLever = false;
    }

    void HandleLeverObject(GameObject lever)
    {
        ResetHighlight();
        interactableObject = lever;
        interactableoriginalMaterial = interactableObject.GetComponent<Renderer>().material;
        interactableObject.GetComponent<Renderer>().material = highlightMaterial;
        isLookingAtLever = true;
        leverObject = lever;
        canpickup = false;
        isLookingAtDoor = false;
    }

    void ResetInteraction()
    {
        ResetHighlight();
        canpickup = false;
        ObjectIwantToPickUp = null;
        isLookingAtDoor = false;
        isLookingAtLever = false;
        doorObject = null;
        leverObject = null;
    }

    void ResetHighlight()
    {
        if (ObjectIwantToPickUp != null && originalMaterial != null)
        {
            ObjectIwantToPickUp.GetComponent<Renderer>().material = originalMaterial;
        }

        if (interactableObject != null && interactableoriginalMaterial != null)
        {
            interactableObject.GetComponent<Renderer>().material = interactableoriginalMaterial;
        }
    }

    void PickUp()
    {
        hasItem = true;
        currentlyHeldObject = ObjectIwantToPickUp;
        currentlyHeldObject.GetComponent<Rigidbody>().isKinematic = true;
        currentlyHeldObject.transform.position = myHands.transform.position;
        currentlyHeldObject.transform.SetParent(player); // Make the object a child of the player
        ResetHighlight();
    }

    void Drop()
    {
        if (currentlyHeldObject == null) return;

        hasItem = false;
        currentlyHeldObject.GetComponent<Rigidbody>().isKinematic = false;
        currentlyHeldObject.transform.parent = null;
        currentlyHeldObject = null;
    }

    void ToggleDoor()
    {
        if (doorObject != null)
        {
            // Check if the door is jammed
            JammedDoor jammedDoor = doorObject.GetComponent<JammedDoor>();
            if (jammedDoor != null)
            {
                // Confirm the player is holding the required object
                if (currentlyHeldObject == null || currentlyHeldObject.name != jammedDoor.requiredObjectName)
                {
                    Debug.Log("The door is jammed and you do not have the required object.");
                    return;
                }
            }

            // Open the door if it isn't jammed or the requirement is met
            if (!openDoors.ContainsKey(doorObject))
            {

                if (jammedDoor != null)
                {
                    MeshRenderer meshRenderer = doorObject.GetComponent<MeshRenderer>();
                    Collider doorCollider = doorObject.GetComponent<Collider>();

                    if (meshRenderer != null) meshRenderer.enabled = false;
                    if (doorCollider != null) doorCollider.enabled = false;

                    Destroy(currentlyHeldObject);
                    Drop();
                }
                else
                {
                    Coroutine doorCoroutine = StartCoroutine(OpenDoorTemporarily(doorObject));
                    openDoors.Add(doorObject, doorCoroutine);
                }
            }
        }
    }

    IEnumerator OpenDoorTemporarily(GameObject door)
    {
        if (door == null) yield break;

        MeshRenderer meshRenderer = door.GetComponent<MeshRenderer>();
        Collider doorCollider = door.GetComponent<Collider>();

        if (meshRenderer != null) meshRenderer.enabled = false;
        if (doorCollider != null) doorCollider.enabled = false;

        yield return new WaitForSeconds(doorOpenTime);

        if (door != null)
        {
            if (meshRenderer != null) meshRenderer.enabled = true;
            if (doorCollider != null) doorCollider.enabled = true;
            openDoors.Remove(door);
        }
    }

    IEnumerator DisplayMessage()
    {
        jammedAlert.SetActive(true); // Show the UI element
        yield return new WaitForSeconds(3); // Wait for a few seconds
        jammedAlert.SetActive(false); // Hide the UI element
    }

    void ToggleLever()
    {
        if (leverObject != null)
        {
            Lever lever = leverObject.GetComponent<Lever>();
            if (lever != null)
            {
                lever.Toggle();
            }
        }
    }
}
