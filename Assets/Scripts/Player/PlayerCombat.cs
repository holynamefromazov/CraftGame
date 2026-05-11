using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private PlayerEquipment _equipment;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Camera _playerCamera;
    private RaycastHit _hitInfo;
    private Ray _ray;

    public void OnAttack()
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
}
