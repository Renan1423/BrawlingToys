using UnityEngine;

namespace BrawlingToys.Actors
{
    public class HitCommand : MonoBehaviour, IHitCommand
    {
        protected GameObject _hitBullet;

        public void Execute(Hitable target)
        {
            switch (target.GetTargetType())
            {
                case (HitableType.Player):
                    HitPlayer(target.GetComponent<PlayerHit>());
                    break;
                case(HitableType.Wall):
                    HitWall();
                    break;
            }
        }

        public GameObject GetBullet()
        {
            return _hitBullet;
        }

        public void SetBullet(GameObject bullet)
        {
            _hitBullet = bullet;
        }

        protected virtual void HitPlayer(PlayerHit playerHit)
        {
        }

        protected virtual void HitWall()
        {
        }
    }
}