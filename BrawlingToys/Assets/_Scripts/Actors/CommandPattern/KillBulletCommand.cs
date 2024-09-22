using BrawlingToys.Network;
using System;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class KillBulletCommand : ICommand
    {
        private Player _bulletOwner;

        private const string BULLET_NAME = "KillerBullet";

        public KillBulletCommand(Transform firePoint, Player bulletOwner)
        {
            _bulletOwner = bulletOwner;
        }

        public void Execute() => _bulletOwner.SpawnBulletServerRpc(BULLET_NAME, _bulletOwner.PlayerId);
    }
}

