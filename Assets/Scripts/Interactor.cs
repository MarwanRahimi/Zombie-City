using UnityEngine;
using TMPro;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionRadius = 1.0f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private TextMeshProUGUI _promptText;

    private IInteractable _interactable;
    private bool _isDisplayed = false;
    private Collider[] _colliders = new Collider[3]; // Declare the _colliders array

    private void Update()
    {
        // Check if _interactionPoint is not null before accessing its position
        if (_interactionPoint != null)
        {
            int numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionRadius, _colliders, _interactableMask);

            if (numFound > 0)
            {
                _interactable = _colliders[0]?.GetComponent<IInteractable>();

                if (_interactable != null)
                {
                    if (!_isDisplayed)
                    {
                        if (_interactable is Vehicle vehicle)
                        {
                            vehicle.UpdatePrompt();
                        }
                        SetUp(_interactable.Prompt);
                    }

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        InteractWithInteractable();
                    }
                }
            }
            else
            {
                if (_interactable != null)
                {
                    _interactable = null;
                }
                if (_isDisplayed)
                {
                    Close();
                }
            }
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

    private void InteractWithInteractable()
    {
        if (_interactable != null)
        {
            _interactionRadius = 0.0f;
            _interactable.Interact(this);
            Invoke("ResetInteractionRadius", 0.8f);
        }
    }

    public void SetUp(string promptText)
    {
        _promptText.text = promptText;
        _isDisplayed = true;
    }

    public void Close()
    {
        _isDisplayed = false;
        _promptText.text = "";
    }

    private void ResetInteractionRadius()
    {
        _interactionRadius = 1.2f;
    }
}
