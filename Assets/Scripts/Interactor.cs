using UnityEngine;
using TMPro;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionRadius = 1.0f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private int _numFound;
    private readonly Collider[] _colliders = new Collider[3];
    private IInteractable _interactable;
    [SerializeField] private TextMeshProUGUI _promptText;
    private bool isDisplayed = false;

    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionRadius, _colliders, _interactableMask);

        if (_numFound > 0)
        {
            _interactable = _colliders[0].GetComponent<IInteractable>();

            if (_interactable != null)
            {
                if (!isDisplayed)
                {
                    if (_interactable is Vehicle)
                    {
                        Vehicle vehicle = (Vehicle)_interactable;
                        vehicle.UpdatePrompt();
                    }
                    SetUp(_interactable.Prompt);
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    _interactionRadius = 0.0f;

                    _interactable.Interact(this);

                    // Restore the original interaction radius after a delay (you can adjust the duration as needed)
                    Invoke("ResetInteractionRadius", 0.7f);
                }
            }
        }
        else
        {
            if (_interactable != null) _interactable = null;
            if (isDisplayed) Close();
        }
    }

    private void OnDrawGizmos()
    {
        if (_interactionPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_interactionPoint.position, _interactionRadius);
        }
    }

    public void SetUp(string promptText)
    {
        _promptText.text = promptText;
        isDisplayed = true;
    }

    public void Close()
    {
        isDisplayed = false;
        _promptText.text = "";
    }

    private void ResetInteractionRadius()
    {
        _interactionRadius = 1.2f;
    }
}
