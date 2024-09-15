using BrawlingToys.Network;
using System;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class KillBulletCommand : ICommand
    {
        private Transform _firePoint;
        private Player _bulletOwner;

        private const string BULLET_NAME = "KillerBullet";

        public KillBulletCommand(Transform firePoint, Player bulletOwner)
        {
            _firePoint = firePoint;
            _bulletOwner = bulletOwner;
            NetworkSpawner.LocalInstance.WhenObjectSpawnedOnServer.AddListener(InitializeBullet);
        }

        public void Execute()
        {
            //GameObject bullet = Object.Instantiate(Resources.Load("KillerBullet"), _firePoint.position, _firePoint.rotation) as GameObject;
            
            NetworkSpawner.LocalInstance.InstantiateOnServer(BULLET_NAME, _firePoint.position, _firePoint.rotation);
        }

        private void InitializeBullet(string bulletName, GameObject bullet)
        {
            if (bulletName != BULLET_NAME)
                return;

            Debug.Log(_bulletOwner);

            BaseBullet newBullet = bullet.GetComponent<BaseBullet>();
            newBullet.Initialize(_bulletOwner);
        }
    }
}

