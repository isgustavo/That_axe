using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class AxeSoundBehaviour : MonoBehaviour
{
    [SerializeField]
    protected AudioClip grabAudioClip;
    [SerializeField]
    protected AudioClip travellingAudioClip;
    [SerializeField]
    protected AudioClip returningAudioClip;
    [SerializeField]
    protected AudioClip collisionAudioClip;
    [SerializeField]
    protected AudioClip monsterCollisionAudioClip;

    protected AudioSource audioSource;

    public void PlayGrabAudio()
    {
        audioSource.Stop();
        audioSource.clip = grabAudioClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayTravellingAudio()
    {
        audioSource.Stop();
        audioSource.clip = travellingAudioClip;
        audioSource.loop = true;
        audioSource.Play();    
    }

    public void PlayAttackAudio()
    {
        audioSource.Stop();
        audioSource.clip = travellingAudioClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayReturningAudio()
    {
        audioSource.Stop();
        audioSource.clip = returningAudioClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayCollisionAudio()
    {
        audioSource.Stop();
        audioSource.clip = collisionAudioClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayMonsterCollisionAudio()
    {
        audioSource.Stop();
        audioSource.clip = monsterCollisionAudioClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    protected void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

}
