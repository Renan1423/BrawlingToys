using UnityEngine;
using UnityEngine.Events;

namespace BrawlingToys.Actors
{
    public interface IDamageable
    {
        public int Health { get; set; }

        public void Damage(Player sender);

        public void Knockback(GameObject sender);
    }
}
