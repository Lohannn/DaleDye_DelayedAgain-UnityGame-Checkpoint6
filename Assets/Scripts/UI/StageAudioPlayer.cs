using UnityEngine;

public class StageAudioPlayer : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioClip[] audioClips;

    public int LOSE = 0;
    public int WIN = 1;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int clipIndex)
    {
        audioSource.PlayOneShot(audioClips[clipIndex]);
    }
}
