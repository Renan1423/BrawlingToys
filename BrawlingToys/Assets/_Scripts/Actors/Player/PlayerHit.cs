using Unity.Netcode;
using UnityEngine;

namespace BrawlingToys.Actors
{
    [RequireComponent(typeof(Player))]
    public class PlayerHit : Hitable
    {
        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        public override void GetHit(GameObject sender, IHitCommand hitCommand)
        {
            base.GetHit(sender, hitCommand);
        }

        public override HitableType GetTargetType()
        {
            targetType = HitableType.Player;
            return targetType;
        }

        public void PlayerDie()
        {
            DieServerRpc();
        }

        public void PlayerKnockback()
        {

        }

        [ServerRpc(RequireOwnership = false)]
        private void DieServerRpc()
        {
            DieClientRpc();
        }

        [ClientRpc]
        private void DieClientRpc()
        {
            DieInCurrentState();
        }

        private void DieInCurrentState()
        {
            if (_player.CurrentState == null) return;

            _player.CurrentState.HandleDie();
        }
    }
}
