using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private InputReader _inputReader;
    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;
    private Vector2 _moveInput;
    private Vector3 _desiredMove;
    private float _verticalVelocity;
    private bool isGrounded => _characterController.isGrounded;

    private void OnEnable()
    {
        _inputReader.OnMoveEvent += HandleMoveInput;
        _inputReader.OnJumpEvent += HandleJumpInput;
    }

    private void OnDisable()
    {
        _inputReader.OnMoveEvent -= HandleMoveInput;
        _inputReader.OnJumpEvent -= HandleJumpInput;
    }

    private void Update()
    {
        Move(Time.deltaTime);
    }

    private void LateUpdate()
    {
    }

    // Реализация интерфейса
    public void HandleMoveInput(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }

    public void HandleJumpInput()
    {
        if (isGrounded)
        {
            _verticalVelocity = Mathf.Sqrt(-2f * _gravity * _jumpHeight);
        }
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
}
