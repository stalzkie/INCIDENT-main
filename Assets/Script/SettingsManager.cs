using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    void Start()
    {
        // Ensure the cursor is visible and unlocked in the Settings scene
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void BackToGame()
    {
        // Return to the game scene
        SceneManager.LoadScene("INCIDENT_prototype"); // Replace with the actual name of your game scene
    }
}
