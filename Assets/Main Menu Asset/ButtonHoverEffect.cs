using UnityEngine;
using UnityEngine.EventSystems; // Necessary for handling UI events
using System.Collections; // Necessary for IEnumerator

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Define scale factors
    private Vector3 normalScale = new Vector3(1, 1, 1);
    private Vector3 hoverScale = new Vector3(1.03f, 1.03f, 1.03f);
    private float duration = 0.3f;  // Duration in seconds over which the scale change should occur

    // This method is called when the cursor enters the element's area
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();  // Stop any existing scale animations
        StartCoroutine(ScaleOverTime(hoverScale, duration));  // Start scaling up
    }

    // This method is called when the cursor exits the element's area
    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();  // Stop any existing scale animations
        StartCoroutine(ScaleOverTime(normalScale, duration));  // Start scaling down
    }

    // Coroutine to scale the object over a specified duration
    IEnumerator ScaleOverTime(Vector3 targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;  // Current scale at the start of the animation
        float time = 0;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);  // Smoothly interpolate from startScale to targetScale over 'duration' seconds
            time += Time.deltaTime;  // Increment time by the delta between frames
            yield return null;  // Wait until the next frame to continue
        }

        transform.localScale = targetScale;  // Ensure the target scale is set after interpolation
    }
}
