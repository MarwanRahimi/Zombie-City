using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    // Configuration parameters
    [SerializeField] AudioClip[] playlist;
    [SerializeField] [Range(0, 1)] float[] playlistVolume;

    // State variables
    int playlistIndex = 0;

    // Cached references
    AudioSource myAudioSource;

    private void Awake() {
        ManageSingleton();
    }

    private void ManageSingleton()  {
        int instanceCount = FindObjectsOfType(GetType()).Length;

        if (instanceCount > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        PlaySoundtracks();
    }

    private void PlaySoundtracks() {

        if (playlist.Length < 1) { return; }

        if (!myAudioSource.isPlaying) {

            myAudioSource.PlayOneShot(playlist[playlistIndex], playlistVolume[playlistIndex]);
            playlistIndex++;
            if (playlistIndex >= playlist.Length) {
                playlistIndex = 0;
            }
        }

    }

}
