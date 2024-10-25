using BrawlingToys.Core;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Windows;

namespace BrawlingToys.Actors
{
    [RequireComponent(typeof(Player))]
    public class PlayerHit : Hitable
    {
        public Player Player { get => _player; }

        private Player _player;

        [Header("Knockback Stuff")]
        private CountdownTimer _knockbackTimer;
        [SerializeField] private float _knockbackDuration;
        private float _knockbackPower;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void Update()
        {
            if (_knockbackTimer != null && _knockbackTimer.IsRunning)
            {
                _knockbackTimer.Tick(Time.deltaTime);
            }
        }

        public override void GetHit(GameObject sender, IHitCommand hitCommand)
        {
            base.GetHit(sender, hitCommand);
            DieServerRpc(); 
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

        private void DieInCurrentState()
        {
            if (_player.CurrentState == null) return;

            _player.CurrentState.HandleDie();
        }

        public void PlayerKnockback(Bullet hitBullet)
        {
            _knockbackPower = hitBullet.Speed;
            _knockbackDuration = hitBullet.Rb.mass/_player.Rb.mass;

            _knockbackTimer = new CountdownTimer(_knockbackDuration);

            if (IsOwner)
            {
                _knockbackTimer.OnTimerStart = () => _player.Inputs.TogglePlayerMap(false);
                _knockbackTimer.OnTimerStop = () => _player.Inputs.TogglePlayerMap(true);
            }

            KnockBackServerRpc(hitBullet.transform.forward);
        }

        public void PlayerKnockback(Player hitPlayer) {
            _knockbackPower = hitPlayer.Rb.velocity.magnitude;
            _knockbackDuration = hitPlayer.Rb.mass / _player.Rb.mass;

            _knockbackTimer = new CountdownTimer(_knockbackDuration);

            if (IsOwner) {
                _knockbackTimer.OnTimerStart = () => _player.Inputs.TogglePlayerMap(false);
                _knockbackTimer.OnTimerStop = () => _player.Inputs.TogglePlayerMap(true);
            }

            KnockBackServerRpc(hitPlayer.transform.forward);
        }

        private void ApplyKnockback(Vector3 bulletFoward)
        {
            _knockbackTimer.Start();

            _player.Rb.AddForce(_knockbackPower * bulletFoward, ForceMode.Impulse);
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

        [ServerRpc(RequireOwnership = false)]
        private void KnockBackServerRpc(Vector3 bulletFoward)
        {
            KnockBackClientRpc(bulletFoward);
        }

        [ClientRpc]
        private void KnockBackClientRpc(Vector3 bulletFoward)
        {
            ApplyKnockback(bulletFoward);
        }
    }
}
