using BrawlingToys.Core;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;

namespace BrawlingToys.Actors
{
    [RequireComponent(typeof(Player))]
    public class PlayerHit : Hitable
    {
        public UnityEvent<int> OnPlayerLifeChange;

        public Player Player { get => _player; }
        public int CurrentLife { get => _currentLife; }
        public int MaxLife { get => _maxLife; }
        public float CollateralChance { get => _collateralDamageChance; }

        private Player _player;
        private int _maxLife = 1;
        private int _currentLife;

        [Header("Knockback Stuff")]
        private CountdownTimer _knockbackTimer;
        [SerializeField] private float _knockbackDuration;
        private float _knockbackPower;

        [Header("Collateral Stuff")]
        [SerializeField] private float _collateralDamageChance;

        [Header("Invulnerability Stuff")]
        [SerializeField] private float _invulnerabilityDuration;
        private CountdownTimer _invulnerabilityTimer;

        private void Awake()
        {
            _player = GetComponent<Player>();

            _invulnerabilityTimer = new(_invulnerabilityDuration);

            ResetHitStats();
        }

        public void ResetHitStats()
        {
            if (IsOwner)
            {
                _currentLife = _maxLife;

                _knockbackTimer.Stop();
                _knockbackTimer.Reset();

                _invulnerabilityTimer.Stop();
                _invulnerabilityTimer.Reset();
            }
        }

        private void Update()
        {
            if (_knockbackTimer != null && _knockbackTimer.IsRunning)
                _knockbackTimer.Tick(Time.deltaTime);

            if (_invulnerabilityTimer.IsRunning)
                _invulnerabilityTimer.Tick(Time.deltaTime);
        }

        public override void GetHit(GameObject sender, IHitCommand hitCommand)
        {
            if (_invulnerabilityTimer.IsRunning)
                return;
            
            _player.MyKiller = sender.GetComponent<Player>();
            base.GetHit(sender, hitCommand);
        }

        public override HitableType GetTargetType()
        {
            targetType = HitableType.Player;
            return targetType;
        }

        public void PlayerDie()
        {
            Debug.Log("Calling Die Server Rpc");
            DieServerRpc();
            
            // _currentLife--;
            // OnPlayerLifeChange?.Invoke(_currentLife);
            
            // if(_currentLife <= 0)
            // {
            //     Debug.Log("Calling Die Server Rpc");
            //     DieServerRpc();
            // }
            // else
            //     _invulnerabilityTimer.Start();
        }

        public bool PlayerTookCollateralDamage() {
            float teste = Random.Range(0.0f, 100.0f);

            if (teste <= _collateralDamageChance)
                return true;
            else
                return false;
        }

        private void DieInCurrentState()
        {
            Debug.Log("Die in current state");
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

        public void SetPlayerMaxLife(int value) 
        {
            _maxLife = value;
            _currentLife = _maxLife; 
            // ResetHitStats();
        }

        //public void PlayerKnockback(Player hitPlayer) {
        //    _knockbackPower = hitPlayer.Rb.velocity.magnitude;
        //    _knockbackDuration = hitPlayer.Rb.mass / _player.Rb.mass;

        //    _knockbackTimer = new CountdownTimer(_knockbackDuration);

        //    if (IsOwner) {
        //        _knockbackTimer.OnTimerStart = () => _player.Inputs.TogglePlayerMap(false);
        //        _knockbackTimer.OnTimerStop = () => _player.Inputs.TogglePlayerMap(true);
        //    }

        //    KnockBackServerRpc(hitPlayer.transform.forward);
        //}

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
