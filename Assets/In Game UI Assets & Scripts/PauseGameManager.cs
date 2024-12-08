using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGameManager : MonoBehaviour
{
    public GameObject pauseCanvas; // Drag your Pause Canvas here
    public GameObject settingsOverlay; // Drag your SettingsOverlay prefab here
    public GameObject battery; // Drag the battery GameObject here
    public GameObject oxygen; // Drag the oxygen GameObject here
    public GameObject compass; // Drag the compass GameObject here
    public GameObject objectiveList; // Drag the objective list GameObject here
    public PlayerMove playerController; // Reference to your Player Controller script
    public CharacterController charController; // Reference to Unity's Character Controller
    public MonoBehaviour cameraMovementScript; // Reference to your camera movement script
    public UnityEngine.UI.Button continueButton; // Drag your Continue Button here in the Inspector 
    public UnityEngine.UI.Button restartButton; // Drag your Restart Button here in the Inspector

    private bool isPaused = false;
    private float pauseToggleCooldown = 0.2f; // Cooldown for toggling pause
    private float lastPauseToggleTime;

    void Start()
    {
        // Ensure all overlays are hidden at the start
        if (pauseCanvas != null) pauseCanvas.SetActive(false);
        if (settingsOverlay != null) settingsOverlay.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Assign the Continue button's click event to the ResumeGame function
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(ResumeGame);
        }

        // Assign the Restart button's click event to the RestartGame function
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartToBeginning);
        }
    }

    void Update()
    {
        // Toggle pause when "Escape" is pressed with a cooldown
        if (Input.GetKeyDown(KeyCode.Escape) && Time.unscaledTime - lastPauseToggleTime > pauseToggleCooldown)
        {
            lastPauseToggleTime = Time.unscaledTime;

            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        if (pauseCanvas != null) pauseCanvas.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f; // Freeze the game

        // Disable player movement and camera movement
        if (playerController) playerController.enabled = false;
        if (charController) charController.enabled = false;
        if (cameraMovementScript) cameraMovementScript.enabled = false;

        // Hide compass, battery, oxygen, and objective list
        if (compass != null) compass.SetActive(false);
        if (battery != null) battery.SetActive(false);
        if (oxygen != null) oxygen.SetActive(false);
        if (objectiveList != null) objectiveList.SetActive(false);
    }

    public void ResumeGame()
    {
        isPaused = false;

        if (pauseCanvas != null) pauseCanvas.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f; // Resume the game

        // Enable player movement and camera movement
        if (playerController) playerController.enabled = true;
        if (charController) charController.enabled = true;
        if (cameraMovementScript) cameraMovementScript.enabled = true;

        // Show compass, battery, oxygen, and objective list
        if (compass != null) compass.SetActive(true);
        if (battery != null) battery.SetActive(true);
        if (oxygen != null) oxygen.SetActive(true);
        if (objectiveList != null) objectiveList.SetActive(true);
    }

    public void OpenSettings()
    {
        if (settingsOverlay != null)
        {
            settingsOverlay.SetActive(true); // Enable the settings overlay
            if (pauseCanvas != null) pauseCanvas.SetActive(false); // Hide the pause menu
        }
    }

    public void CloseSettings()
    {
        if (settingsOverlay != null)
        {
            settingsOverlay.SetActive(false); // Disable the settings overlay
        }

        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(true); // Re-enable the pause menu
        }
    }

    public void RestartToBeginning()
    {
        Debug.Log("Restart button clicked!"); // Debug message for testing
        Time.timeScale = 1f; // Reset time scale
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Replace with the actual name of your first scene
    }

    public void QuitToMainMenu()
    {
        Debug.Log("Quit button clicked!"); // Debug message for testing
        Time.timeScale = 1f; // Reset time scale
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu"); // Replace with the actual name of your main menu scene
    }
}
