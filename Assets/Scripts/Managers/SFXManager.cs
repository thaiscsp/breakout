using UnityEngine;

public class SFXManager : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip gameOver, hit, reset, win;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

}
