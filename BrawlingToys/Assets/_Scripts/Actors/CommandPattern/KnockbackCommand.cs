using BrawlingToys.Network;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class KnockbackCommand : IHitCommand
    {
        private GameObject _hitSender;

        public void Execute(IHitable target)
        {
            target.GetHit(GetSender(), this);
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

