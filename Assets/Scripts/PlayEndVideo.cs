using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Video;

public class PlayEndVideo : MonoBehaviour
{
    [SerializeField]
    private Health healthComponent;
    void Awake()
    {

        healthComponent.OnDie += OnDie;
    }

    private void OnDie()
    {
        GetComponent<VideoPlayer>().Play();
    }
}
