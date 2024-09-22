using UnityEngine;

namespace BrawlingToys.Actors
{
    public class KillerBullet : BaseBullet
    {
        public override void OnTriggerEnter(Collider other)
        {
            if(!ValidCollision(other)) return; 
            
            if (other.TryGetComponent(out IDamageable hit))
                hit.Damage(_bulletOwner);

            DestroyBulletServerRpc();
        }
    }
}

