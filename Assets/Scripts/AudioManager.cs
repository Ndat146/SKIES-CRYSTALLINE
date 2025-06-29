using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip homeMusic;   
    public AudioClip gameMusic;
    public AudioClip winSound;  
    public AudioClip loseSound;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject); 
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHomeMusic()
    {
        audioSource.clip = homeMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayGameMusic()
    {
        audioSource.clip = gameMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayWinSound()
    {
        if (audioSource != null && winSound != null)
        {
            audioSource.PlayOneShot(winSound);
        }
    }

    public void PlayLoseSound()
    {
        if (audioSource != null && loseSound != null)
        {
            audioSource.PlayOneShot(loseSound);
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
