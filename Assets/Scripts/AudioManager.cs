using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header ("--------- Audio Source ---------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("--------- Audio Clip ---------")]
    public AudioClip backgroundmusic;
    public AudioClip btnclick;
    public AudioClip btnhover;

    public void Start()
    {
        musicSource.clip = backgroundmusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void ButtonClicked()
    {
        SFXSource.PlayOneShot(btnclick);
    }

    public void ButtonHover()
    {
        SFXSource.PlayOneShot(btnhover);
    }
}
