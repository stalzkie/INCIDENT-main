using UnityEngine;
using UnityEngine.EventSystems; // Required for UI event handling
using UnityEngine.UI; // Required for UI Components
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Example scale factors for hover effect
    private Vector3 normalScale = new Vector3(1, 1, 1);
    private Vector3 hoverScale = new Vector3(1.3f, 1.3f, 1.3f);

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(5);
    }

    public void LoadGame()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void Settings()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Implementation of IPointerEnterHandler
    public void OnPointerEnter(PointerEventData eventData)
    {
        // This will be called when the cursor is hovered over the GameObject this script is attached to.
        transform.localScale = hoverScale;
    }

    // Implementation of IPointerExitHandler
    public void OnPointerExit(PointerEventData eventData)
    {
        // This will be called when the cursor stops hovering over the GameObject.
        transform.localScale = normalScale;
    }
}
