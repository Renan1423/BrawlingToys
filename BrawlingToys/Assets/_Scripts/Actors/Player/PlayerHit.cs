using UnityEngine;

namespace BrawlingToys.Actors
{
    public class PlayerHit : IHitable
    {
        private Player _player;

        public PlayerHit(Player player)
        {
            _player = player;
        }

        public void GetHit(GameObject sender, IHitCommand hitCommand)
        {
            hitCommand.Execute(this);
        }
    }
}
