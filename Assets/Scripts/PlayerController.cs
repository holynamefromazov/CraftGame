using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, InputControll.IPlayerActions
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    private InputControll inputControll;
    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector3 moveDirection;
    private bool isGrounded;

    private void Awake()
    {
        inputControll = new InputControll();
        rb = GetComponent<Rigidbody>();

        // Назначаем этот скрипт как обработчик
        inputControll.Player.SetCallbacks(this);
    }

    private void OnEnable()
    {
        inputControll.Player.Enable();
    }

    private void OnDisable()
    {
        inputControll.Player.Disable();
    }

    private void Update()
    {
        Move();
    }

    // Реализация интерфейса
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded && context.performed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            Debug.Log("Attack");
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
            Debug.Log("Interact");
    }

    private void Move()
    {
        moveDirection.Set(moveInput.x, 0, moveInput.y);
        if (moveDirection.magnitude > 1.01f)
        {
            moveDirection.Normalize();
            Debug.Log($"Move Input: {moveInput} and Move Direction: {moveDirection}");
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
}