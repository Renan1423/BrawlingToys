using Unity.Netcode;
using UnityEngine;
using BrawlingToys.Network;
using System.Linq;
using System;

namespace BrawlingToys.Actors
{
    public class NetworkWeaponShooter : NetworkBehaviour
    {
        private Transform _firePoint; 

        private bool _initialized = false; 

        public void Init(Transform firePoint)
        {
            _firePoint = firePoint;
            _initialized = true;  
        }

        public void SpawnBullet(string bulletName, ulong bulletOwnerPlayerId, float bulletPower) => SpawnBulletServerRpc(bulletName, bulletOwnerPlayerId,
            bulletPower); 
        
        [ServerRpc(RequireOwnership = false)]
        private void SpawnBulletServerRpc(string bulletName, ulong bulletOwnerPlayerId, float bulletPower)
        {
            if (!_initialized) throw new Exception("NetworkWeaponShooter was not initialized!");

            var bullet = NetworkSpawner
                .LocalInstance
                .InstantiateOnServer(bulletName, _firePoint.position, _firePoint.rotation)
                .GetComponent<Bullet>();

            InitBulletClientRpc(bulletOwnerPlayerId, bullet.NetworkObjectId, bulletPower);
        }

        [ClientRpc]
        private void InitBulletClientRpc(ulong bulletOwnerPlayerId, ulong bulletNetworkObjectId, float bulletPower)
        {
            var bullet = NetworkManager.Singleton.SpawnManager.SpawnedObjects[bulletNetworkObjectId].GetComponent<Bullet>();

            var owner = Player.Instances.First(p => p.PlayerId == bulletOwnerPlayerId);
            bullet.Initialize(owner, bulletPower);

            Debug.Log(owner.PlayerId);
        }
    }
}
