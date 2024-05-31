using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySFX : MonoBehaviour {

    // Configuration parameters
    [SerializeField] float audioRange = 30f;
    [SerializeField] float idleMinDelay = 3f;
    [SerializeField] float idleMaxDelay = 6f;
    [SerializeField] float chasingMinDelay = 1.5f;
    [SerializeField] float chasingMaxDelay = 3f;
    [SerializeField] float attackingMinDelay = 0.5f;
    [SerializeField] float attackingMaxDelay = 1f;
    [SerializeField] AudioClip[] idleSFX;
    [SerializeField] [Range(0, 1)] float[] idleSFXVolume;
    [SerializeField] AudioClip[] chasingSFX;
    [SerializeField] [Range(0, 1)] float[] chasingSFXVolume;
    [SerializeField] AudioClip[] attackingSFX;
    [SerializeField] [Range(0, 1)] float[] attackingSFXVolume;
    [SerializeField] AudioClip[] deathSFX;
    [SerializeField] [Range(0, 1)] float[] deathSFXVolume;

    // State variables
    string currentAnimationState = "idle";
    string previousAnimationState = "idle";
    int audioClipIndex = 0;
    float randomDelay = 0f;

    // Cached references
    AudioSource myAudioSource;
    EnemyAI myEnemyAI;
    PlayerHealth targetPlayerHealth;

    // Start is called before the first frame update
    void Start() {
        myAudioSource = GetComponent<AudioSource>();
        myEnemyAI = GetComponent<EnemyAI>();
        targetPlayerHealth = myEnemyAI.GetTargetTransform().GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update() {
        HandleAudioForAnimation();
    }

    private void HandleAudioForAnimation() {
        if (targetPlayerHealth.GetIsDead()) {
            myAudioSource.Stop();
            enabled = false;
            return;
        }

        if (currentAnimationState != "death") {
            currentAnimationState = myEnemyAI.GetAnimationState();
        }

        if ((myAudioSource.isPlaying && currentAnimationState == previousAnimationState) || myEnemyAI.GetDistanceToTarget() > audioRange) {
            return;
        }

        else if (currentAnimationState != previousAnimationState) {
            if (currentAnimationState == "chasing" && previousAnimationState == "idle") {
                PlayNewAudioClipWithoutDelay(chasingSFX, chasingSFXVolume);
            }
            else if (currentAnimationState == "attacking") {
                PlayNewAudioClipWithoutDelay(attackingSFX, attackingSFXVolume);
            }
            else if (currentAnimationState == "death") {
                PlayNewAudioClipWithoutDelay(deathSFX, deathSFXVolume);
                enabled = false;
            }
            previousAnimationState = currentAnimationState;
        }

        else if (!myAudioSource.isPlaying){
            if (currentAnimationState == "idle") {
                PlayNewAudioClipWithDelay(idleSFX, idleSFXVolume, idleMinDelay, idleMaxDelay);
            }
            else if (currentAnimationState == "chasing") {
                PlayNewAudioClipWithDelay(chasingSFX, chasingSFXVolume, chasingMinDelay, chasingMaxDelay);
            }
            else if (currentAnimationState == "attacking") {
                PlayNewAudioClipWithDelay(attackingSFX, attackingSFXVolume, attackingMinDelay, attackingMaxDelay);
            }
        }
    }

    private void PlayNewAudioClipWithoutDelay(AudioClip[] SFX, float[] SFXVolume) {
        myAudioSource.Stop();
        audioClipIndex = Random.Range(0, SFX.Length);
        myAudioSource.clip = SFX[audioClipIndex];
        myAudioSource.volume = SFXVolume[audioClipIndex];
        myAudioSource.Play();
    }

    private void PlayNewAudioClipWithDelay(AudioClip[] SFX, float[] SFXVolume, float minDelay, float maxDelay) {
        myAudioSource.Stop();
        audioClipIndex = Random.Range(0, SFX.Length);
        randomDelay = Random.Range(minDelay, maxDelay);
        myAudioSource.clip = SFX[audioClipIndex];
        myAudioSource.volume = SFXVolume[audioClipIndex];
        myAudioSource.PlayDelayed(randomDelay);
    }


    public void SetCurrentAnimationState(string animationState) {
        currentAnimationState = animationState;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, audioRange);
    }


}
