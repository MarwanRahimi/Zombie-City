using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDamage : MonoBehaviour {

    // Configuration parameters
    [SerializeField] Image hurtImage;
    [SerializeField] float disappearTime = 2f;
    [SerializeField] float maxAlpha = 130f;

    // State variables
    bool isFading = false;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (isFading) {
            FadeHurtOverlay();
        }
    }

    private void FadeHurtOverlay() {
        float alphaFadeThisFrame = Time.deltaTime * maxAlpha / disappearTime;
        var tempColor = hurtImage.color;
        tempColor.a -= alphaFadeThisFrame;

        if (tempColor.a <= 0) {
            tempColor.a = 0;
            isFading = false;
        }

        hurtImage.color = tempColor;
    }


    public void DisplayHurtOverlay() {
        var tempColor = hurtImage.color;
        tempColor.a = maxAlpha;
        hurtImage.color = tempColor;

        isFading = true;
    }

    public void DisableHurtOverlayOnDeath() {
        hurtImage.enabled = false;
        isFading = false;
        enabled = false;
    }

}
