using UnityEngine;

public class ReusableTrigger : MonoBehaviour
{
    public GameObject targetObject; // The object to activate/deactivate

    void OnTriggerEnter(Collider other)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true); // Activate the target object
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false); // Deactivate the target object
        }
    }
}
