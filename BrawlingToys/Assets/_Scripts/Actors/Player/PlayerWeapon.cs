using System;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class PlayerWeapon
    {
        public event EventHandler<Vector3> OnUpdateAimRotation;

        private Player _player;
        private Transform _firePoint;

        private float _aimSmoothRate;

        private LayerMask _groundLayerMask;
        private RaycastHit _hitInfo;

        private NetworkWeaponShooter _networkShooter; 

        public PlayerWeapon(Player player, Transform firePoint, float aimSmoothRate, LayerMask groundLayerMask, NetworkWeaponShooter networkShooter)
        {
            _player = player;
            _firePoint = firePoint;
            _aimSmoothRate = aimSmoothRate;
            _groundLayerMask = groundLayerMask;
            
            _networkShooter = networkShooter; 
            _networkShooter.Init(_firePoint); 
        }

        public void Update()
        {
            HandleAim();
        }

        private void HandleAim()
        {
            if (!_player.Inputs.IsActive)
                return;

            Ray ray = Camera.main.ScreenPointToRay(_player.Inputs.GetLookVector());
            if (Physics.Raycast(ray, out _hitInfo, float.MaxValue, _groundLayerMask))
            {
                _player.transform.forward = Vector3.Slerp(_player.transform.forward, _hitInfo.point - _player.transform.position, 
                    _aimSmoothRate * Time.deltaTime);
                OnUpdateAimRotation?.Invoke(this, _player.transform.forward);
            }
        }

        public void Shoot(ulong ownerPlayerId) 
        => _networkShooter.SpawnBullet("Bullet", ownerPlayerId);
    }
}
