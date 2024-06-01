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


    //public static AudioManager instance;

    public void Start()
    {
        musicSource.clip = backgroundmusic;
        musicSource.Play();
    }


    //creates a bug with the SFX
    //public void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(this);
    //    }
    //    else
    //    {
    //        Destroy(this);
    //    }
    //}

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
