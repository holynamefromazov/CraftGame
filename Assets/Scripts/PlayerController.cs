using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, InputControl.IPlayerActions
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float cameraSensitivity = 1f;
    [SerializeField] private Camera playerCamera;

    private InputControl InputControl;
    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector3 moveDirection;
    private Vector2 lookDelta;
    private Vector2 cameraRotation;
    private bool isGrounded;

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
            Debug.Log("Attack");
        DetectDevice(context);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
            Debug.Log("Interact");
        DetectDevice(context);
    }

    public void OnCameraСontrol(InputAction.CallbackContext context)
    {
        lookDelta = context.ReadValue<Vector2>();
        DetectDevice(context);
    }
    private void CameraControl()
    {
        cameraRotation.x += lookDelta.x * cameraSensitivity;
        cameraRotation.y += lookDelta.y * cameraSensitivity;
        cameraRotation.y = Mathf.Clamp(cameraRotation.y, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(-cameraRotation.y, 0, 0);
        transform.localRotation = Quaternion.Euler(0, cameraRotation.x, 0);
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
        bool isGamepad = device is Gamepad;
        Debug.Log("Device used:" + (isGamepad ? "Gamepad" : "Keyboard/Mouse"));
    }

}