using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private InputReader _inputReader;
    [Header("Settings")]
    [SerializeField] private float _mouseCameraRotationSensitivity = 1f;
    [SerializeField] private float _gamepadCameraRotationSensitivity = 1f;
    [SerializeField] private float _verticalRotationLimit = 60f;

    private Vector2 _lookDelta;
    private Vector2 _cameraRotation;
    private RaycastHit _hitInfo;
    private Ray _ray;
    public void OnEnable()
    {
        _inputReader.OnCameraControlEvent += OnCameraControl;
    }
    public void OnDisable()
    {
        _inputReader.OnCameraControlEvent -= OnCameraControl;
    }

    private void LateUpdate()
    {
        CameraControl();
    }
    public void OnCameraControl(Vector2 lookDelta)
    {
        _lookDelta = lookDelta;

    }
    private void CameraControl()
    {

        _cameraRotation += _lookDelta * (_inputReader.IsGamepadInput ? _gamepadCameraRotationSensitivity : _mouseCameraRotationSensitivity);

        _cameraRotation.y = Mathf.Clamp(_cameraRotation.y, -_verticalRotationLimit, _verticalRotationLimit);

        transform.localRotation = Quaternion.Euler(0f, _cameraRotation.x, 0f);
        _playerCamera.transform.localRotation = Quaternion.Euler(-_cameraRotation.y, 0f, 0f);

    }
}
