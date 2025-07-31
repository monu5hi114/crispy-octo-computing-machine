using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip eatClip;
    public AudioClip wallHitClip;
    public AudioClip trimClip;        
    public AudioClip doubleScoreClip; 

    [Header("Pitch Settings")]
    public float normalPitch = 1f;
    public float highPitch = 1.3f;
    public float lowPitch = 0.7f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    private void Start()
    {
        if (bgmSource != null && !bgmSource.isPlaying)
        {
            bgmSource.loop = true;
            bgmSource.pitch = normalPitch;
            bgmSource.Play();
        }
    }

    

    public void PlayEatSound()
    {
        PlaySFX(eatClip);
    }

    public void PlayWallHitSound()
    {
        PlaySFX(wallHitClip);
    }

    public void PlayTrimSound()
    {
        PlaySFX(trimClip);
    }

    public void PlayDoubleScoreSound()
    {
        PlaySFX(doubleScoreClip);
    }

    private void PlaySFX(AudioClip clip)
    {
        Debug.Log("sfxplayed");
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip);
    }

    

    public void SetNormalSpeed()
    {
        if (bgmSource != null)
            bgmSource.pitch = normalPitch;
    }

    public void SetHighSpeed()
    {
        if (bgmSource != null)
            bgmSource.pitch = highPitch;
    }

    public void SetSlowMotion()
    {
        if (bgmSource != null)
            bgmSource.pitch = lowPitch;
    }
    public void BGMOF()
    {
        bgmSource.Pause();
    }
}
