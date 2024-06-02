using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Image transitionImage;
    public float transitionSpeed = 1f;
    [SerializeField]
    private string level;

    private void Start()
    {
        transitionImage.gameObject.SetActive(false);
        FadeOut();
    }

    public void LoadNextScene(string sceneName)
    {
        FadeIn();

        Invoke("LoadSceneDelayed", transitionSpeed);
    }

    private void LoadSceneDelayed()
    {
        SceneManager.LoadScene(level);
    }

    private void FadeIn()
    {
        transitionImage.gameObject.SetActive(true);
        transitionImage.canvasRenderer.SetAlpha(0f);
        transitionImage.CrossFadeAlpha(1f, transitionSpeed, false);
    }

    private void FadeOut()
    {
        transitionImage.CrossFadeAlpha(0f, transitionSpeed, false);
    }
}