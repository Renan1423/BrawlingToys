using BrawlingToys.Network;
using System;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class KillBulletCommand : ICommand
    {
        private Transform _firePoint;

        public KillBulletCommand(Transform firePoint)
        {
            _firePoint = firePoint;
        }

        public void Execute()
        {
            Debug.Log("KillerBullet");
            //GameObject bullet = Object.Instantiate(Resources.Load("KillerBullet"), _firePoint.position, _firePoint.rotation) as GameObject;
            NetworkSpawner.LocalInstance.InstantiateOnServer("KillerBullet", _firePoint.position, _firePoint.rotation);
        }
    }
}

