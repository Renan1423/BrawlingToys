using UnityEngine;
using UnityEngine.Events;

namespace BrawlingToys.Actors
{
    public interface IHitable
    {
        public void GetHit(GameObject sender, IHitCommand hitCommand);
    }
}
