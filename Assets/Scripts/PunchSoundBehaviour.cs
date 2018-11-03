using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PunchSoundBehaviour : MonoBehaviour
{

    [SerializeField]
    public AudioClip punchAudioClip;

    protected AudioSource audioSource;

    protected void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPunchAudio()
    {
        audioSource.Stop();
        audioSource.clip = punchAudioClip;
        audioSource.loop = false;
        audioSource.Play();
    }
}
