using BrawlingToys.Network;
using System;
using System.Linq;
using Unity.Netcode;
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

        public PlayerWeapon(Player player, Transform firePoint, float aimSmoothRate, LayerMask groundLayerMask)
        {
            _player = player;
            _firePoint = firePoint;
            _aimSmoothRate = aimSmoothRate;
            _groundLayerMask = groundLayerMask;
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

        [ServerRpc(RequireOwnership = false)]
        public void SpawnBulletServerRpc(string bulletName, ulong bulletOwnerPlayerId)
        {
            var bullet = NetworkSpawner
                .LocalInstance
                .InstantiateOnServer(bulletName, _firePoint.position, _firePoint.rotation)
                .GetComponent<Bullet>();

            var owner = Player.Instances.First(p => p.PlayerId == bulletOwnerPlayerId);

            bullet.Initialize(owner);
        }
    }
}
