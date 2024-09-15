using UnityEngine;

namespace BrawlingToys.Actors
{
    public class PushBullet : BaseBullet
    {
        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            if (other.TryGetComponent(out IDamageable hit))
                hit.Knockback(gameObject);

            DestroyBulletServerRpc();
        }
    }
}