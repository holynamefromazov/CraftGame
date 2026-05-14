using UnityEngine;
using UnityEngine.Assertions;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private PlayerEquipment _equipment;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private PlayerCamera _playerCamera;
    private RaycastHit _hitInfo;
    private void Start()
    {
        Assert.IsNotNull(_playerCamera, "PlayerCamera reference is not assigned.");
        Assert.IsNotNull(_inputReader, "InputReader reference is not assigned.");
        Assert.IsNotNull(_equipment, "PlayerEquipment reference is not assigned.");
    }
    private void OnEnable()
    {
        _inputReader.OnAttackEvent += OnAttack;
    }
    private void OnDisable()
    {
        _inputReader.OnAttackEvent -= OnAttack;
    }
    private void OnAttack()
    {
        if (_equipment?.CurrentWeapon == null) return;
        if (_playerCamera.GetRayCastHit(out _hitInfo, _equipment.CurrentWeapon.Range))
        {
            var damageable = _hitInfo.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(_equipment.CurrentWeapon.Damage);
            }
        }
    }
}
