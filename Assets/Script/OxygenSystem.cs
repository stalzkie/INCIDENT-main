using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class OxygenSystem : MonoBehaviour
{
    [Header("Oxygen Settings")]
    public float maxOxygen = 100f;
    public float currentOxygen;
    public float oxygenDepletionRate = 5f; // Oxygen lost per second
   
    [Header("UI Elements")]
    public Slider oxygenSlider;
    public Image oxygenBarFill;
    public TextMeshProUGUI gameOverText; // Add reference for game over text

    [Header("Player References")]
    public MonoBehaviour cameraMovementScript; // Reference to your camera movement script
   
    [Header("Color Settings")]
    public Color normalColor = Color.cyan;
    public Color lowOxygenColor = Color.red;
    public float lowOxygenThreshold = 30f;
   
    private bool isGameOver = false;

    void Start()
    {
        // Initialize oxygen to max
        currentOxygen = maxOxygen;
       
        // Set up the slider
        if (oxygenSlider != null)
        {
            oxygenSlider.maxValue = maxOxygen;
            oxygenSlider.value = currentOxygen;
        }
       
        // Set initial color
        if (oxygenBarFill != null)
        {
            oxygenBarFill.color = normalColor;
        }

        // Hide game over text at start
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isGameOver) return;
        // Decrease oxygen over time
        if (currentOxygen > 0)
        {
            currentOxygen -= oxygenDepletionRate * Time.deltaTime;
            currentOxygen = Mathf.Max(0f, currentOxygen); // Prevent negative values
        }
        else if (!isGameOver)
        {
            GameOver();
        }
        // Update UI
        UpdateOxygenUI();
    }

    void UpdateOxygenUI()
    {
        if (oxygenSlider != null)
        {
            oxygenSlider.value = currentOxygen;
            // Update color based on oxygen level
            if (oxygenBarFill != null)
            {
                float oxygenPercentage = (currentOxygen / maxOxygen) * 100f;
                if (oxygenPercentage <= lowOxygenThreshold)
                {
                    // Lerp between low oxygen color and normal color for smooth transition
                    float t = oxygenPercentage / lowOxygenThreshold;
                    oxygenBarFill.color = Color.Lerp(lowOxygenColor, normalColor, t);
                }
                else
                {
                    oxygenBarFill.color = normalColor;
                }
            }
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over - Out of Oxygen!");
       
        // Show game over text
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            // gameOverText.text = "GAME OVER\nOut of Oxygen!";
        }

        // Disable camera movement
        if (cameraMovementScript != null)
        {
            cameraMovementScript.enabled = false;
        }

        // Reload the current scene after a delay
        StartCoroutine(ReloadSceneAfterDelay(5f));
       
        // Optionally disable player movement
        // If you have a movement script on the player, disable it:
        GetComponent<PlayerMove>().enabled = false;
    }

    IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Call this when player reaches air pocket or surface
    public void RefillOxygen()
    {
        currentOxygen = maxOxygen;
    }

    // Call this when collecting oxygen pickup or power-up
    public void AddOxygen(float amount)
    {
        currentOxygen = Mathf.Min(currentOxygen + amount, maxOxygen);
    }
}