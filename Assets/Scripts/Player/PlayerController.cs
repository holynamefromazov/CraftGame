using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, InputControl.IPlayerActions
{
    [SerializeField] private PlayerEquipment _equipment;
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private CharacterController _characterController;
    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpHight = 2f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _interactionRange = 1f;
    [SerializeField] private LayerMask _interactionLayerMask = 1 << 6; // Default to "Interactive" layer
    [SerializeField] private float _mouseCameraRotationSensitivity = 1f;
    [SerializeField] private float _gamepadCameraRotationSensitivity = 1f;

    private InputControl _inputControl;
    private Vector2 _moveInput;
    private Vector3 _desiredMove;
    private float _verticalVelocity;
    private Vector2 _lookDelta;
    private Vector2 _cameraRotation;
    private bool isGamepadInput;
    private bool isGrounded => _characterController.isGrounded;
    private RaycastHit _hitInfo;
    private Ray _ray;

    private void Awake()
    {
        _inputControl = new InputControl();

        // Назначаем этот скрипт как обработчик
        _inputControl.Player.SetCallbacks(this);
    }

    private void OnEnable()
    {
        _inputControl.Player.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        _inputControl.Player.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void FixedUpdate()
    {
        Move(Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        CameraControl();
    }

    // Реализация интерфейса
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        DetectDevice(context);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded && context.performed)
        {
            _verticalVelocity = Mathf.Sqrt(-2f * _gravity * _jumpHight);
        }

        DetectDevice(context);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_equipment?.CurrentWeapon == null) return;
            _ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(_ray, out _hitInfo, _equipment.CurrentWeapon.Range))
            {
                var damageable = _hitInfo.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(_equipment.CurrentWeapon.Damage);
                }
            }
        }
        DetectDevice(context);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {

        if (context.performed)
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
        DetectDevice(context);
    }

    public void OnCameraControl(InputAction.CallbackContext context)
    {
        _lookDelta = context.ReadValue<Vector2>();
        DetectDevice(context);
    }
    private void CameraControl()
    {

        _cameraRotation += _lookDelta * (isGamepadInput ? _gamepadCameraRotationSensitivity : _mouseCameraRotationSensitivity);

        _cameraRotation.y = Mathf.Clamp(_cameraRotation.y, -60f, 60f);

        transform.localRotation = Quaternion.Euler(0f, _cameraRotation.x, 0f);
        _playerCamera.transform.localRotation = Quaternion.Euler(-_cameraRotation.y, 0f, 0f);

    }
    private void Move(float deltaTime)
    {
        _desiredMove = (transform.forward * _moveInput.y) + (transform.right * _moveInput.x);
        if (_desiredMove.magnitude > 1.01f)
        {
            _desiredMove.Normalize();
        }

        _verticalVelocity += _gravity * deltaTime;
        if (isGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = -2f;
        }
        _characterController.Move((_desiredMove * _moveSpeed + Vector3.up * _verticalVelocity) * deltaTime);
    }

    private void DetectDevice(InputAction.CallbackContext context)
    {
        var device = context.control.device;
        isGamepadInput = device is Gamepad;
    }

}