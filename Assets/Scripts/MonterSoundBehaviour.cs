
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class MonterSoundBehaviour : MonoBehaviour {

    [SerializeField]
    protected AudioClip walkingAudioClip;
    [SerializeField]
    protected AudioClip dissolveAudioClip;
    [SerializeField]
    protected AudioClip AttakingAudioClip;

    protected AudioSource audioSource;

    public void PlayWalkingAudio()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        audioSource.Stop();
        audioSource.clip = walkingAudioClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayDissolveAudio()
    {
        audioSource.Stop();
        audioSource.clip = dissolveAudioClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayAttakingAudio()
    {
        audioSource.Stop();
        audioSource.clip = AttakingAudioClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
