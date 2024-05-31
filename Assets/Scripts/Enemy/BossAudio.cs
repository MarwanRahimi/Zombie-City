using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAudio : MonoBehaviour {

    // Configuration parameters
    [SerializeField] AudioClip chasingSFX;
    [SerializeField] [Range(0, 1)] float chasingSFXVolume;
    [SerializeField] AudioClip attackingSFX;
    [SerializeField] [Range(0, 1)] float attackingSFXVolume;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathSFXVolume;
    [SerializeField] AudioClip chasingDrums;
    [SerializeField] [Range(0, 1)] float chasingDrumsVolume;
    [SerializeField] float startingChasingDrumLength = 2.5f;
    [SerializeField] float minChasingDrumsLength = 1f;
    [SerializeField] float speedUpChasingDrums = 0.1f;

    // State variables
    string currentAnimationState = "idle";
    string previousAnimationState = "idle";
    bool playDrums = false;
    float currentChasingDrumsLength;

    // Cached references
    AudioSource myAudioSource;
    EnemyAI myEnemyAI;
    PlayerHealth targetPlayerHealth;

    // Start is called before the first frame update
    void Start() {
        myAudioSource = GetComponent<AudioSource>();
        myEnemyAI = GetComponent<EnemyAI>();
        targetPlayerHealth = myEnemyAI.GetTargetTransform().GetComponent<PlayerHealth>();
        currentChasingDrumsLength = startingChasingDrumLength;
    }

    // Update is called once per frame
    void Update() {
        HandleAudio();
    }

    private void HandleAudio() {
        if (targetPlayerHealth.GetIsDead()) {
            StopAudio();
            enabled = false;
            return;
        }

        if (currentAnimationState != "death") {
            currentAnimationState = myEnemyAI.GetAnimationState();
        }

        if (playDrums && !myAudioSource.isPlaying) {
            PlayTheDrum();
        }

        if (myAudioSource.isPlaying && currentAnimationState == previousAnimationState) {
            return;
        }

        else if (currentAnimationState == "chasing") {
            playDrums = true;
            myAudioSource.PlayOneShot(chasingSFX, chasingSFXVolume);
        }

        else if (currentAnimationState == "attacking") {
            playDrums = false;
            StopAudio();
            AudioSource.PlayClipAtPoint(attackingSFX, targetPlayerHealth.transform.position, attackingSFXVolume);
            enabled = false;
        }

        else if (currentAnimationState == "death") {
            playDrums = false;
            CancelInvoke();
            StopAudio();
            myAudioSource.PlayOneShot(deathSFX, deathSFXVolume);
            enabled = false;
        }

        previousAnimationState = currentAnimationState;

    }

    private void PlayTheDrum() {
        myAudioSource.PlayOneShot(chasingDrums, chasingDrumsVolume);
        Invoke("StopAudio", currentChasingDrumsLength);
        currentChasingDrumsLength -= speedUpChasingDrums;
        if (currentChasingDrumsLength < minChasingDrumsLength) {
            currentChasingDrumsLength = minChasingDrumsLength;
        }
    }

    private void StopAudio() {
        myAudioSource.Stop();
    }

    public void SetCurrentAnimationStateBoss(string animationState) {
        currentAnimationState = animationState;
    }

}
