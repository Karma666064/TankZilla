using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Configuration")]
    public AudioClip musicClip; // La musique de cette scène
    public float volume = 0.5f;
    public bool loopMusic = true;
    public bool playOnStart = true;
    
    [Header("Fade Settings (optionnel)")]
    public bool useFadeIn = false;
    public float fadeInDuration = 1f;
    
    private AudioSource audioSource;
    private float targetVolume;
    
    void Awake()
    {
        // Créer l'AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.loop = loopMusic;
        audioSource.playOnAwake = false;
        
        targetVolume = volume;
        
        // Si on utilise le fade, commencer à volume 0
        if (useFadeIn)
        {
            audioSource.volume = 0f;
        }
        else
        {
            audioSource.volume = volume;
        }
    }
    
    void Start()
    {
        if (playOnStart && musicClip != null)
        {
            PlayMusic();
        }
    }
    
    void Update()
    {
        // Gérer le fade in
        if (useFadeIn && audioSource.isPlaying && audioSource.volume < targetVolume)
        {
            audioSource.volume += (targetVolume / fadeInDuration) * Time.deltaTime;
            
            if (audioSource.volume >= targetVolume)
            {
                audioSource.volume = targetVolume;
                useFadeIn = false; // Fade terminé
            }
        }
    }
    
    public void PlayMusic()
    {
        if (audioSource != null && musicClip != null)
        {
            audioSource.Play();
            Debug.Log($"Musique lancée : {musicClip.name}");
        }
    }
    
    public void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
    
    public void PauseMusic()
    {
        if (audioSource != null)
        {
            audioSource.Pause();
        }
    }
    
    public void ResumeMusic()
    {
        if (audioSource != null)
        {
            audioSource.UnPause();
        }
    }
    
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        targetVolume = volume;
        
        if (audioSource != null && !useFadeIn)
        {
            audioSource.volume = volume;
        }
    }
    
    // Méthode pour faire un fade out avant de changer de scène (optionnel)
    public void FadeOut(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }
    
    private System.Collections.IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }
        
        audioSource.volume = 0f;
        audioSource.Stop();
    }
}