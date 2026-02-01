using UnityEngine;

public enum SoundEffect
{
    Frog,
    Bull,
    Owl,
    Deer,
    Snake,
    Pit,
    Walk
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundlist;
    private static SoundManager instance;
    private AudioSource audioSource;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public static void PlaySound(SoundEffect effect_, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundlist[(int)effect_], volume);
    }
}
