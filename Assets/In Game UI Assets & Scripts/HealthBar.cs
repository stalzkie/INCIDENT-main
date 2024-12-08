using UnityEngine;
using UnityEngine.UI; // For UI components
using TMPro; // For TextMeshPro

public class OxygenBar : MonoBehaviour
{
    public Slider healthSlider; // Reference to the Slider UI
    public float maxHealth = 100f; // Maximum health
    public float currentHealth; // Current health
    public float decreaseSpeed = 20f; // Speed at which health decreases (units per second)
    public Image fillImage; // Reference to the fill area of the slider (for color change)

    public Image darkOverlay; // Darkening overlay to indicate low oxygen
    public TextMeshProUGUI oxygenLowText; // Flickering "Oxygen Low" text (TMP)
    public float fadeSpeed = 0.5f; // Speed at which the overlay darkens

    private bool isLowOxygen = false; // Tracks if oxygen is below 20%
    private bool isFlickering = false; // Tracks if the text is flickering

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        // Set initial color
        UpdateFillColor();

        // Initialize dark overlay and text
        if (darkOverlay != null)
        {
            Color overlayColor = darkOverlay.color;
            overlayColor.a = 0; // Fully transparent at start
            darkOverlay.color = overlayColor;
        }

        if (oxygenLowText != null)
        {
            oxygenLowText.gameObject.SetActive(false); // Hide text initially
        }
    }

    void Update()
    {
        // Slowly decrease health over time
        if (currentHealth > 0)
        {
            currentHealth -= decreaseSpeed * Time.deltaTime; // Decrease health at a fixed rate
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Prevent health from going below 0
            UpdateHealthBar();
        }

        // Trigger low oxygen effects if health drops below 20%
        if (currentHealth <= 20f && !isLowOxygen)
        {
            isLowOxygen = true;
            StartLowOxygenEffects();
        }

        // Gradually darken the screen as health decreases
        if (isLowOxygen)
        {
            DarkenScreen();
        }
    }

    public void UpdateHealthBar()
    {
        // Update the slider value to match current health
        healthSlider.value = currentHealth;

        // Update the color of the health bar
        UpdateFillColor();
    }

    private void UpdateFillColor()
    {
        // Calculate the health percentage
        float healthPercentage = currentHealth / maxHealth;

        // Transition from white to red as health decreases
        fillImage.color = Color.Lerp(Color.red, Color.white, healthPercentage);
    }

    void StartLowOxygenEffects()
    {
        if (oxygenLowText != null)
        {
            oxygenLowText.gameObject.SetActive(true); // Show the "Oxygen Low" text
            StartCoroutine(FlickerText());
        }
    }

    void DarkenScreen()
    {
        if (darkOverlay != null)
        {
            Color overlayColor = darkOverlay.color;
            overlayColor.a = Mathf.Lerp(overlayColor.a, 1f, fadeSpeed * Time.deltaTime); // Gradually darken
            darkOverlay.color = overlayColor;
        }
    }

    private System.Collections.IEnumerator FlickerText()
    {
        isFlickering = true;
        while (currentHealth <= 20f)
        {
            if (oxygenLowText != null)
            {
                oxygenLowText.enabled = !oxygenLowText.enabled; // Toggle text visibility
            }
            yield return new WaitForSeconds(0.5f); // Flicker interval
        }
        isFlickering = false;
    }
}
