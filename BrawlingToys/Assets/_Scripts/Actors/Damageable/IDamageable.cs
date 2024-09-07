using UnityEngine;

namespace BrawlingToys.Actors
{
    public interface IDamageable
    {
        public int Health { get; set; }

        public void Damage();

        public void Knockback(GameObject sender);
    }
}
