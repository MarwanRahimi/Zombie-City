using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Video;

public class PlayEndVideo : MonoBehaviour
{
    [SerializeField]
    private Health healthComponent;

    [SerializeField]
    private AudioSource level4Audio;

    private float delayTime = 3.0f;

    private float elapsedTime;

    private bool shouldPlayVideo;

    void Awake()
    {
        healthComponent.OnDie += OnDie;
    }

    private void Start()
    {
        if(level4Audio == null)
            level4Audio = GameObject.Find("Level4Music").GetComponent<AudioSource>();   
    }

    private void Update()
    {
        if (shouldPlayVideo)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= delayTime)
            {
                GetComponent<VideoPlayer>().Play();
                PlayerPrefs.SetInt("PlayerScore", 0);
                shouldPlayVideo = false;
            }
        }
    }

    private void OnDie()
    {
        shouldPlayVideo = true;
        level4Audio.Stop();
    }


}
