using UnityEngine;
using UnityEngine.Assertions;

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
    private void Start()
    {
        Assert.IsNotNull(_playerCamera, "PlayerCamera reference is not assigned.");
        Assert.IsNotNull(_inputReader, "InputReader reference is not assigned.");
    }
    private void OnEnable()
    {
        _inputReader.OnCameraControlEvent += OnCameraControl;
    }
    private void OnDisable()
    {
        _inputReader.OnCameraControlEvent -= OnCameraControl;
    }

    private void LateUpdate()
    {
        CameraControl();
    }
    private void OnCameraControl(Vector2 lookDelta)
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

    public Ray GetCameraRay()
    {
        return _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    }
    public bool GetRayCastHit(out RaycastHit hitInfo, float range, LayerMask layerMask)
    {
        return Physics.Raycast(GetCameraRay(), out hitInfo, range, layerMask);
    }

    public bool GetRayCastHit(out RaycastHit hitInfo, float range = 100f)
    {
        return GetRayCastHit(out hitInfo, range, ~0);
    }

}
