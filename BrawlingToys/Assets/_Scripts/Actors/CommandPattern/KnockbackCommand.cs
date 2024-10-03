using BrawlingToys.Network;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class KnockbackCommand : IHitCommand
    {
        private GameObject _hitSender;

        public void Execute(Hitable target)
        {
            switch (target.GetTargetType())
            {
                case (HitableType.Player):
                    target.GetComponent<PlayerHit>().PlayerKnockback();
                    break;
                case (HitableType.Wall):
                    //
                    break;
            }
        }

        public GameObject GetSender()
        {
            return _hitSender;
        }

        public void SetSender(GameObject sender)
        {
            _hitSender = sender;
        }
    }
}

