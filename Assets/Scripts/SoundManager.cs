using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource SoundAudioSource;

    public AudioClip ButtonClip, WinClip, DrawClip, AddObject;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(int number)
    {
        if (number == 0)
        {
            SoundAudioSource.PlayOneShot(ButtonClip);
        }
        else if (number == 1)
        {
            SoundAudioSource.PlayOneShot(WinClip);
        }
        else if (number == 2)
        {
            SoundAudioSource.PlayOneShot(DrawClip);
        }
        else if (number == 4)
        {
            SoundAudioSource.PlayOneShot(AddObject);
        }
    }
}
