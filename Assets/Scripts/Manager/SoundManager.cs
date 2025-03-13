using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    AudioSource myAudio;
    public AudioClip soundScream;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        myAudio = GetComponent<AudioSource>(); 
    }

    public void SoundScream()
    {
        myAudio.PlayOneShot(soundScream);
    }
}
