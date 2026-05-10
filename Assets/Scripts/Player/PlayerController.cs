using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, InputControl.IPlayerActions
{
    [SerializeField] private PlayerEquipment equipment;
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float interactionRange = 1f;
    [SerializeField] private string interactionLayerMask = "Interactive";
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float mouseCameraRotationSensitivity = 1f;
    [SerializeField] private float gamepadCameraRotationSensitivity = 1f;

    private InputControl InputControl;
    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector3 moveDirection;
    private Vector2 lookDelta;
    private Vector2 cameraRotation;
    private bool isGamepadInput;
    private bool isGrounded;
    private RaycastHit hitInfo;
    private Ray ray;

    private void Awake()
    {
        InputControl = new InputControl();
        rb = GetComponent<Rigidbody>();

        // Назначаем этот скрипт как обработчик
        InputControl.Player.SetCallbacks(this);
    }

    private void OnEnable()
    {
        InputControl.Player.Enable();
    }

    private void OnDisable()
    {
        InputControl.Player.Disable();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraControl();
    }

    // Реализация интерфейса
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        DetectDevice(context);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded && context.performed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
        DetectDevice(context);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (equipment?.CurrentWeapon == null) return;
            ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out hitInfo, equipment.CurrentWeapon.Range))
            {
                var damageable = hitInfo.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(equipment.CurrentWeapon.Damage);
                }
            }
        }
        DetectDevice(context);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out hitInfo, interactionRange, LayerMask.GetMask(interactionLayerMask)))
            {
                if (hitInfo.collider.TryGetComponent(out IInteractable interactable))
                {
                    if (interactable.Interact(this))
                        return;
                }

                if (hitInfo.collider.TryGetComponent(out ICollectable collectable))
                {
                    if (collectable.Collect(inventory))
                        return;
                }

                Debug.Log("No interactable or collectable object in range.");
            }
        }
        DetectDevice(context);
    }

    public void OnCameraControl(InputAction.CallbackContext context)
    {
        lookDelta = context.ReadValue<Vector2>();
        DetectDevice(context);
    }
    private void CameraControl()
    {

        cameraRotation += lookDelta * (isGamepadInput ? gamepadCameraRotationSensitivity : mouseCameraRotationSensitivity);

        cameraRotation.y = Mathf.Clamp(cameraRotation.y, -60f, 60f);

        transform.localRotation = Quaternion.Euler(0f, cameraRotation.x, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(-cameraRotation.y, 0f, 0f);

    }
    private void Move()
    {
        moveDirection.Set(moveInput.x, 0, moveInput.y);
        if (moveDirection.magnitude > 1.01f)
        {
            moveDirection.Normalize();
        }

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void DetectDevice(InputAction.CallbackContext context)
    {
        var device = context.control.device;
        isGamepadInput = device is Gamepad;
    }

}