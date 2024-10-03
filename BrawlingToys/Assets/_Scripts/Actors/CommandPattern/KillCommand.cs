using UnityEngine;

namespace BrawlingToys.Actors
{
    public class KillCommand : HitCommand
    {
        protected override void HitPlayer(PlayerHit playerHit)
        {
            playerHit.PlayerDie();
        }

        protected override void HitWall()
        {
        }
    }
}

