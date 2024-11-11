using UnityEngine;

namespace BrawlingToys.Actors {
    public class CollateralCommand : KillCommand {

        protected override void HitPlayer(PlayerHit playerHit) {
            playerHit.PlayerDie();
        }

        protected override void HitWall() {
        }
    }
}