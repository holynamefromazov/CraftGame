using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, InputControl.IPlayerActions
{
    private InputControl _inputControl;

    public event System.Action<Vector2> OnMoveEvent;
    public event System.Action OnJumpEvent;
    public event System.Action OnAttackEvent;
    public event System.Action OnInteractEvent;
    public event System.Action<Vector2> OnCameraControlEvent;
    public event System.Action OnDeviceChangeEvent;
    public bool IsGamepadInput { get; private set; }
    private void Awake()
    {
        _inputControl = new InputControl();

        // Назначаем этот скрипт как обработчик
        _inputControl.Player.SetCallbacks(this);
    }
    public void OnEnable()
    {
        _inputControl.Player.Enable();
    }
    public void OnDisable()
    {
        _inputControl.Player.Disable();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
        DetectDevice(context);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnJumpEvent?.Invoke();
        }
        DetectDevice(context);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnAttackEvent?.Invoke();
        }
        DetectDevice(context);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            OnInteractEvent?.Invoke();
        }
        DetectDevice(context);
    }

    public void OnCameraControl(InputAction.CallbackContext context)
    {
        OnCameraControlEvent?.Invoke(context.ReadValue<Vector2>());
        DetectDevice(context);
    }

    private void DetectDevice(InputAction.CallbackContext context)
    {
        bool isCurrentlyGamepad = context.control.device is Gamepad;
        if (isCurrentlyGamepad != IsGamepadInput)
        {
            IsGamepadInput = isCurrentlyGamepad;
            OnDeviceChangeEvent?.Invoke();
        }
    }

}