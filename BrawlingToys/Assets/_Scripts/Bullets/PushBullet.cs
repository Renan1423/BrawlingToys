using BrawlingToys.Actors;
using UnityEngine;

namespace BrawlingToys.Bullets
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