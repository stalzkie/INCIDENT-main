using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortcutScript : MonoBehaviour
{
    public JammedDoor jammedScript; // The object to activate/deactivate

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Alternate Route found");
        if (jammedScript != null)
        {
            Destroy(jammedScript); // Activate the target object
        }
    }
}
