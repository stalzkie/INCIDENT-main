using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("--------------Audio Source--------------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource1; // For SFX1
    [SerializeField] private AudioSource SFXSource2; // For SFX2
    [SerializeField] private AudioSource SFXSource3; // For SFX3

    [Header("--------------Audio Sound--------------")]
    public AudioClip music;
    public AudioClip SFX1;
    public AudioClip SFX2;
    public AudioClip SFX3;

    private static AudioManager instance; // Singleton reference to ensure one instance

    private void Awake()
    {
        // Singleton pattern to ensure only one AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }
    }

    private void Start()
    {
        // Play the background music
        if (musicSource != null && music != null)
        {
            musicSource.clip = music;
            musicSource.loop = true; // Optional: Enable looping for background music
            musicSource.Play();
        }

        // Play SFX1 immediately
        PlaySFX(SFXSource1, SFX1);

        // Start coroutines for periodic sound effects
        StartCoroutine(PlaySFX2EveryTwoMinutes());
        StartCoroutine(PlaySFX3StartingFromThreeMinutes());

        // Subscribe to scene load events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from scene load events to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is the In Game UI scene
        if (scene.name == "In Game UI") // Replace with the actual scene name
        {
            Destroy(gameObject); // Destroy the AudioManager when entering this scene
        }
    }

    // Method to play a specific SFX using a dedicated AudioSource
    private void PlaySFX(AudioSource source, AudioClip clip)
    {
        if (source != null && clip != null)
        {
            source.clip = clip;
            source.Play();
        }
    }

    // Coroutine to play SFX2 every 2 minutes
    private IEnumerator PlaySFX2EveryTwoMinutes()
    {
        while (true)
        {
            yield return new WaitForSeconds(120f); // Wait for 2 minutes
            PlaySFX(SFXSource2, SFX2);
        }
    }

    // Coroutine to play SFX3 every 5 minutes starting from 3 minutes
    private IEnumerator PlaySFX3StartingFromThreeMinutes()
    {
        yield return new WaitForSeconds(30f); // Initial wait of 3 minutes
        while (true)
        {
            PlaySFX(SFXSource3, SFX3);
            yield return new WaitForSeconds(80f); // Wait for 5 minutes
        }
    }
}
