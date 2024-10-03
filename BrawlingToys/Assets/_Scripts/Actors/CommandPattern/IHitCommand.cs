using UnityEngine;

namespace BrawlingToys.Actors
{
    public interface IHitCommand
    {
        public void Execute(Hitable target);
        public GameObject GetBullet();
        public void SetBullet(GameObject bullet);
    }
}