using BrawlingToys.Network;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class PushBulletCommand : ICommand
    {
        private Transform _firePoint;

        public PushBulletCommand(Transform firePoint)
        {
            _firePoint = firePoint;
        }

        public void Execute()
        {
            //GameObject bullet = Object.Instantiate(Resources.Load("PusherBullet"), _firePoint.position, _firePoint.rotation) as GameObject;
            NetworkSpawner.LocalInstance.InstantiateOnServer("PusherBullet", _firePoint.position, _firePoint.rotation); 
        }
    }
}

