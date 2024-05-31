using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSFX : MonoBehaviour {

    // Configuration parameters
    [SerializeField] AudioClip walkingSFX;
    [SerializeField] [Range(0, 1)] float walkingSFXVolume = 1f;
    [SerializeField] AudioClip runningSFX;
    [SerializeField] [Range(0, 1)] float runningSFXVolume = 1f;
    [SerializeField] float jumpWaitTime = 1f;
    [SerializeField] PlayerHealth myPlayerHealth;

    // State variables
    bool wasWalkingPreviousFrame = false;
    bool wasRunningPreviousFrame = false;
    bool isJumping = false;

    // Cached references
    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start() {
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        HandleMovementSFX();
    }

    private void HandleMovementSFX() {

        if (myPlayerHealth.GetIsDead()) {
            myAudioSource.Stop();
            enabled = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            myAudioSource.Stop();
            StartCoroutine(WaitForJump());
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.UpArrow) || 
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow)) {
            
            if (!myAudioSource.isPlaying) {
                wasRunningPreviousFrame = false;
                wasWalkingPreviousFrame = false;
            }

            if (Input.GetKey(KeyCode.LeftShift) && !wasRunningPreviousFrame && !isJumping) {
                myAudioSource.Stop();
                myAudioSource.PlayOneShot(runningSFX, runningSFXVolume);
                wasRunningPreviousFrame = true;
                wasWalkingPreviousFrame = false;
            }

            else if (!Input.GetKey(KeyCode.LeftShift) && !wasWalkingPreviousFrame && !isJumping) {
                myAudioSource.Stop();
                myAudioSource.PlayOneShot(walkingSFX, walkingSFXVolume);
                wasWalkingPreviousFrame = true;
                wasRunningPreviousFrame = false;
            }

        }

        else {
            wasRunningPreviousFrame = false;
            wasWalkingPreviousFrame = false;
            myAudioSource.Stop();
        }
    }

    IEnumerator WaitForJump() {
        isJumping = true;
        yield return new WaitForSecondsRealtime(jumpWaitTime);
        isJumping = false;
    }

}
