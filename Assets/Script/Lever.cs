using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject targetObject; // The object to enable/disable


    [Header("For Exit Doors")]
    public GameObject WinDoor1;
    // public GameObject WinDoor2;
    public GameObject WinDoorIndicator1;
    // public GameObject WinDoorIndicator2;
    public GameObject ExitIndicator;


    public void Toggle()
    {
        if (targetObject != null)
        {
            bool isActive = targetObject.activeSelf;
            targetObject.SetActive(!isActive); // Toggle the active state
        }

        if (WinDoor1 != null)
        {
            WinDoor1.tag = "Door";
            // WinDoor2.tag = "Door";
            WinDoorIndicator1.SetActive(true);
            // WinDoorIndicator2.SetActive(true);
            ExitIndicator.SetActive(true);
        }
        
        gameObject.tag = "Used";
        

        // Optional: Add animations or effects for lever toggle
        Debug.Log("Lever toggled!");
    }
}
