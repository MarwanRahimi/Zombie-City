using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VehicleCheck3 : MonoBehaviour, IInteractable
{
    [SerializeField] private string _currPrompt;
    public string Prompt => _currPrompt;
    [SerializeField] private TextMeshProUGUI _objectiveText;

    public AudioClip failedInteractionClip;
    private AudioSource audioSource;
    [SerializeField] private VideoPlayer player;

    private PlayerMovementFPS playerController;
    private Weapon playerShooting;
    private PauseMenu pauseMenu;

    void Start()
    {
        if (_objectiveText != null)
        {
            _objectiveText.text = "Current Objective: Find the cure!";
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();

        playerController = FindObjectOfType<PlayerMovementFPS>();
        playerShooting = FindObjectOfType<Weapon>();
        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    public void UpdatePrompt()
    {
        var inventory = PlayerInventory.Instance;

        if (!inventory.hasCure)
        {
            _currPrompt = "I need to find the cure.";
        }
        else if (EnemySpawner.Instance.remainingEnemies == 0)
        {
            _currPrompt = "Proceed to next area";
        }
        else if (EnemySpawner.Instance.remainingEnemies <= 25)
        {
            _currPrompt = "I need to kill all remaining zombies!";
        }
    }

    public bool Interact(Interactor interactor)
    {
        var inventory = PlayerInventory.Instance;
        if (inventory.hasCure && EnemySpawner.Instance.remainingEnemies == 0)
        {
            player.transform.parent.gameObject.SetActive(true);
            player.Play();

            Time.timeScale = 0f;

            if (playerController != null)
            {
                playerController.enabled = false;
            }

            if (playerShooting != null)
            {
                playerShooting.enabled = false;
            }

            if (pauseMenu != null)
            {
                pauseMenu.enabled = false;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            StartCoroutine(WaitAndLoadCredits(80f));
            return true;
        }
        else
        {
            if (failedInteractionClip != null && audioSource != null)
            {
                audioSource.clip = failedInteractionClip;
                audioSource.Play();
            }
            return false;
        }
    }

    private IEnumerator WaitAndLoadCredits(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        player.transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1f;

        if (playerController != null)
        {
            playerController.enabled = true;
        }

        if (playerShooting != null)
        {
            playerShooting.enabled = true;
        }

        if (pauseMenu != null)
        {
            pauseMenu.enabled = true;
        }

        SceneManager.LoadScene("Credit");
    }

    public void UpdateObjectiveText()
    {
        var inventory = PlayerInventory.Instance;

        if (inventory.hasCure)
        {
            if (EnemySpawner.Instance.remainingEnemies > 0)
            {
                _objectiveText.text = $"Current Objective: Kill all enemies! \nRemaining enemies:{EnemySpawner.Instance.remainingEnemies}";
            }
            else
            {
                _objectiveText.text = "Current Objective: Return to the vehicle!";
            }
        }
        else
        {
            _objectiveText.text = "Current Objective: Find the cure!";
        }
    }

    public void UpdateEnemyCount()
    {
        UpdateObjectiveText();
    }

    public void CurePickup()
    {
        UpdateObjectiveText();
    }
}
