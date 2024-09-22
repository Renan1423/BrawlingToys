using UnityEngine;

namespace BrawlingToys.Actors
{
    public class KillerBullet : BaseBullet
    {
        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            if (other.TryGetComponent(out IDamageable hit))
                hit.Damage(_bulletOwner);

            DestroyBulletServerRpc();
        }
    }
}

