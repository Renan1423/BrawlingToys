using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public enum HitableType
    {
        Player,
        Wall,
    }

    public class Hitable : NetworkBehaviour, IHitable
    {
        protected HitableType targetType;
        public virtual void GetHit(GameObject sender, IHitCommand hitCommand)
        {
            hitCommand.Execute(this);
        }

        public virtual HitableType GetTargetType()
        {
            return targetType;
        }
    }
}
