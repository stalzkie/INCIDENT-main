using UnityEngine;
using UnityEngine.UI; // For Slider
using TMPro; // For TextMeshPro (optional)

public class FlashlightBattery : MonoBehaviour
{
    public Light flashlight; // Drag the Spotlight component here
    public Slider batterySlider; // Drag the Battery Slider here
    public TextMeshProUGUI batteryText; // Drag the Battery Text (optional)
    public float batteryLife = 100f; // Starting battery percentage
    public float depletionRate = 5f; // Percentage per minute
    public float flickerDuration = 0.1f; // Flicker duration in seconds
    public float minIntensity = 0.2f; // Minimum light intensity at 0% battery
    public float maxIntensity = 1.5f; // Maximum light intensity at 100% battery

    private bool isOn = true; // Flashlight state (on/off)
    private float flickerTimer = 0f; // Timer for flickering
    private float flickerInterval = 3f; // Initial flicker interval

    void Start()
    {
        if (flashlight == null)
        {
            flashlight = GetComponent<Light>();
        }

        if (batterySlider != null)
        {
            batterySlider.maxValue = 100f; // Set slider max value
            batterySlider.value = batteryLife; // Initialize slider value
        }
    }

    void Update()
    {
        // Toggle flashlight on/off with F key
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = !isOn;
            flashlight.enabled = isOn && batteryLife > 0; // Turn the flashlight on/off
        }

        if (isOn)
        {
            HandleBattery();
        }

        UpdateBatteryUI();
    }

    void HandleBattery()
    {
        if (batteryLife <= 0f)
        {
            flashlight.enabled = false; // Turn off flashlight when battery is depleted
            return; // Skip further processing
        }

        // Deplete battery over time
        batteryLife -= depletionRate * Time.deltaTime / 60f; // Convert rate to per-second

        // Clamp battery to 0-100%
        batteryLife = Mathf.Clamp(batteryLife, 0f, 100f);

        // Adjust flashlight intensity based on battery life
        AdjustLightIntensity();

        // Handle flickering logic
        if (batteryLife <= 20f)
        {
            HandleFlickering();
        }
        else
        {
            flashlight.enabled = true; // Ensure flashlight stays on when battery > 20%
        }
    }

    void AdjustLightIntensity()
    {
        // Linearly interpolate intensity based on battery percentage
        flashlight.intensity = Mathf.Lerp(minIntensity, maxIntensity, batteryLife / 100f);
    }

    void HandleFlickering()
    {
        // Set flicker interval based on battery level
        if (batteryLife <= 10f)
        {
            flickerInterval = 1f;
        }
        else
        {
            flickerInterval = 3f;
        }

        // Flicker logic
        flickerTimer += Time.deltaTime;

        if (flickerTimer >= flickerInterval)
        {
            flashlight.enabled = !flashlight.enabled; // Toggle light
            flickerTimer = 0f;

            // Restore the light after a short duration
            if (!flashlight.enabled)
            {
                Invoke(nameof(RestoreLight), flickerDuration);
            }
        }
    }

    void RestoreLight()
    {
        if (batteryLife > 0f)
        {
            flashlight.enabled = true;
        }
    }

    void UpdateBatteryUI()
    {
        if (batterySlider != null)
        {
            batterySlider.value = batteryLife; // Update slider value
        }

        if (batteryText != null)
        {
            batteryText.text = $"{Mathf.RoundToInt(batteryLife)}%"; // Update percentage text
        }
    }
}
