using UnityEngine;

public class PlayerAudioPlayer : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioClip[] audioClips;
    //[SerializeField] private AudioClip alarm;
    //[SerializeField] private AudioClip death;
    //[SerializeField] private AudioClip jump;
    //[SerializeField] private AudioClip pickItem;
    //[SerializeField] private AudioClip useSoda;
    //[SerializeField] private AudioClip useBeer;
    //[SerializeField] private AudioClip useSugar;
    //[SerializeField] private AudioClip useSodaBoost;

    public int ALARM = 0;
    public int DEATH = 1;
    public int JUMP = 2;
    public int PICK_ITEM = 3;
    public int USE_SODA = 4;
    public int USE_BEER = 5;
    public int USE_SUGAR = 6;
    public int USE_SODA_BOOST = 7;
    public int BREAK_GLASS = 8;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int clipIndex)
    {
        audioSource.loop = false;

        if (clipIndex == 0)
        {
            audioSource.loop = true;
        }

        audioSource.PlayOneShot(audioClips[clipIndex]);
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
