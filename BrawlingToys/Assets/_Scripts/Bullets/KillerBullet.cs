using BrawlingToys.Actors;
using UnityEngine;

namespace BrawlingToys.Bullets
{
    public class KillerBullet : BaseBullet 
    {
        public override void OnTriggerEnter(Collider other) 
        {
            base.OnTriggerEnter(other); 

            Debug.Log("Collision");
            if (other.TryGetComponent(out IDamageable hit))
                hit.Damage();
            
            DestroyBulletServerRpc(); 
        }
    }
}

