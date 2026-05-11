using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _interactionRange = 1f;
    [SerializeField] private LayerMask _interactionLayerMask = 1 << 6; // Default to "Interactive" layer
    private RaycastHit _hitInfo;
    private Ray _ray;

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

    public void OnInteract()
    {
        _ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(_ray, out _hitInfo, _interactionRange, _interactionLayerMask))
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
