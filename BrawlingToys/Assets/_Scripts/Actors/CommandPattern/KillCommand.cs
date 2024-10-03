using UnityEngine;

namespace BrawlingToys.Actors
{
    public class KillCommand : IHitCommand
    {
        private GameObject _hitSender;

        public void Execute(Hitable target)
        {
            switch (target.GetTargetType())
            {
                case(HitableType.Player) :
                    target.GetComponent<PlayerHit>().PlayerDie();
                    break;
                case(HitableType.Wall) :
                    // 
                    break;
            }
        }
    }
}

