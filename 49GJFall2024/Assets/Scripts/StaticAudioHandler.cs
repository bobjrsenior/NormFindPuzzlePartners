using UnityEngine;

public class StaticAudioHandler : MonoBehaviour
{
    public static StaticAudioHandler instance;

    private AudioSource audioSource;

    public bool gameBeat = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void PlayClip(AudioClip audioClip, bool loop)
    {
        audioSource.clip = audioClip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}
