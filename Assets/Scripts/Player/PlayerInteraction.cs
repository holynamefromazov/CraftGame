using UnityEngine;
using UnityEngine.Assertions;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private PlayerCamera _playerCamera;
    [SerializeField] private float _interactionRange = 1f;
    [SerializeField] private LayerMask _interactionLayerMask = 1 << 6; // Default to "Interactive" layer
    private RaycastHit _hitInfo;
    private void Start()
    {
        Assert.IsNotNull(_inventory, "PlayerInventory reference is not assigned in PlayerInteraction.");
        Assert.IsNotNull(_inputReader, "InputReader reference is not assigned in PlayerInteraction.");
        Assert.IsNotNull(_playerCamera, "PlayerCamera reference is not assigned in PlayerInteraction.");
    }

    private void OnEnable()
    {
        _inputReader.OnInteractEvent += OnInteract;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        _inputReader.OnInteractEvent -= OnInteract;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnInteract()
    {
        if (_playerCamera.GetRayCastHit(out _hitInfo, _interactionRange, _interactionLayerMask))
        {
            if (_hitInfo.collider.TryGetComponent(out IInteractable interactable))
            {
                if (interactable.Interact(this))
                    return;
            }

            if (_hitInfo.collider.TryGetComponent(out ICollectable collectable))
            {
                if (collectable.Collect(_inventory))
                    return;
            }

            Debug.Log("No interactable or collectable object in range.");
        }

    }
}
