using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionRadius = 1.0f;
    [SerializeField] private LayerMask _interactable;
    [SerializeField] private int _numFound;
    private readonly Collider[] _colliders = new Collider[3];

    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionRadius, _colliders, _interactable);

        if(_numFound > 0)
        {
            var interactable = _colliders[0].GetComponent<IInteractable>();

            if (interactable != null && Keyboard.current.fKey.wasPressedThisFrame) //Add joystick button for interaction later
            {
                interactable.Interact(this);
            }
        }
    }
}
