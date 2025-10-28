using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioClip[] audioClips;

    public int TRAFFIC  = 0;
    public int STAGE = 1;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        PlaySound(TRAFFIC);
    }

    public void PlaySound(int clipIndex)
    {
        audioSource.clip = audioClips[clipIndex];
        audioSource.Play();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }

    public void SlowDownSound()
    {
        audioSource.pitch = 0.5f;
    }

    public void ResetSoundSpeed()
    {
        audioSource.pitch = 1.0f;
    }
}
