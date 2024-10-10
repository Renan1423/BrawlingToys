using UnityEngine;

namespace BrawlingToys.Actors
{
    public class HitCommand : IHitCommand
    {
        protected Bullet _hitBullet;

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

        public Bullet GetBullet()
        {
            return _hitBullet;
        }

        public void SetBullet(Bullet bullet)
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