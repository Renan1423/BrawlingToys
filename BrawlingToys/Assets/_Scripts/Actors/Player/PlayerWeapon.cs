using System;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class PlayerWeapon
    {
        public event EventHandler<Vector2> OnUpdateCursorPosition;
        public event EventHandler<float> OnBulletPowerChange;

        private Player _player;
        private Transform _firePoint;
#nullable enable
        private PlayerHit? _playerHit;
#nullable disable

        private float _aimSmoothRate;

        private LayerMask _groundLayerMask;
        private RaycastHit _hitInfo;

        private NetworkWeaponShooter _networkShooter;

        private float _bulletPower = 0;
        private float _maxBulletPower = 1f;

        public RaycastHit HitInfo { get => _hitInfo; }

        public PlayerWeapon(Player player, Transform firePoint, float aimSmoothRate, LayerMask groundLayerMask, NetworkWeaponShooter networkShooter)
        {
            _player = player;
            _firePoint = firePoint;
            _aimSmoothRate = aimSmoothRate;
            _groundLayerMask = groundLayerMask;
            
            _networkShooter = networkShooter; 
            _networkShooter.Init(_firePoint);

            _player.Stats.OnStatsChanged += Stats_OnStatsChanged;
        }

        private void Stats_OnStatsChanged(StatType obj) {
            if (obj != StatType.HitCommand) {
                return;
            }

            if (_player.Stats.CurrentHitEffect is CollateralCommand) {
                _playerHit = _player.GetComponent<PlayerHit>();
            }
        }

        public void Update()
        {
            OnUpdateCursorPosition?.Invoke(this, _player.Inputs.GetMouseLookVector());
            HandleAim();

            if (_player.Inputs.PlayerInputActions.PlayerMap.Shoot.IsPressed())
            {
                if (_bulletPower >= _maxBulletPower)
                    return;
                _bulletPower += Time.deltaTime;
            }
            else
                _bulletPower = 0;

            // Evento para atualizar HUD
            OnBulletPowerChange?.Invoke(this, _bulletPower/_maxBulletPower);
        }

        private void HandleAim() {
            if (!_player.Inputs.IsActive)
                return;

            if (_player.Inputs.UsingMouseAndKeyboard()) {
                Ray ray = Camera.main.ScreenPointToRay(_player.Inputs.GetMouseLookVector());

                if (Physics.Raycast(ray, out _hitInfo, float.MaxValue, _groundLayerMask)) {
                    if (!_hitInfo.point.Equals(Vector3.zero))
                        _player.transform.forward = Vector3.Slerp(
                            _player.transform.forward,
                            _hitInfo.point - _player.transform.position,
                            _aimSmoothRate * Time.deltaTime
                        );
                }
            } else {
                _player.transform.forward = Vector3.Lerp(
                    _player.transform.forward,
                    new Vector3(_player.Inputs.GetStickLookVector().x, 0f, _player.Inputs.GetStickLookVector().y),
                    _aimSmoothRate * Time.deltaTime
                );
            }
        }

        public void Shoot(ulong ownerPlayerId) {
            if (_playerHit != null) {
                if (_playerHit.PlayerTookCollateralDamage()) {
                    _playerHit.PlayerDie();
                    return;
                }
            }

            _networkShooter.SpawnBullet("Bullet", ownerPlayerId, _bulletPower);
        }
    }
}
