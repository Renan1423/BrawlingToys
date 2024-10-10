using UnityEngine;

namespace BrawlingToys.Actors
{
    public class KnockbackCommand : HitCommand
    {
        protected override void HitPlayer(PlayerHit playerHit)
        {
            playerHit.PlayerKnockback(GetBullet());
        }

        protected override void HitWall()
        {
        }
    }
}
