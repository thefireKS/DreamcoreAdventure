using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    [SerializeField] private AudioSource audioSource;

    public void ChangeMusic(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
