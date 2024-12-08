using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerOverlayUI : MonoBehaviour
{
    public GameObject overlayUI; // Assign your screen overlay UI GameObject here

    void Start()
    {
        if (overlayUI != null)
        {
            overlayUI.SetActive(false); // Ensure the overlay is hidden at the start
        }

        // Hide the cursor initially
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the object entering is the player
        {
            if (overlayUI != null)
            {
                overlayUI.SetActive(true); // Show the screen overlay
                Time.timeScale = 0; // Pause the game

                // Show the cursor
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void OnContinueButton()
    {
        Time.timeScale = 1; // Resume the game
        overlayUI.SetActive(false); // Hide the overlay

        // Hide the cursor again
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart the current scene
    }

    public void OnQuitButton()
    {
        Time.timeScale = 1; // Resume the game (in case it is paused)

        // Show the cursor (optional for main menu navigation)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("Main Menu"); // Load the Main Menu scene (replace "MainMenu" with your menu scene name)
    }
}
