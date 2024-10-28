using System;
using UnityEngine;

namespace BrawlingToys.Actors {
    public class MeleeAttackState : State {

        [SerializeField] private Vector3 meleeOffset = Vector3.zero;
        [SerializeField] private Vector3 boxExtents = Vector3.zero;

        protected override void EnterState() {

            //meleeOffset = new Vector3()

            _player.Animations.PlayAnimation(PlayerAnimations.AnimationType.MeleeAttack);
            _player.Animations.OnAnimationEnd.AddListener(WhenMeleeEnds);
            _player.Cooldowns.meleeTimer.Start();

            // Aplicar for�a no player na dire��o de movimento
        }

        protected override void ExitState() {
            _player.Animations.ResetEvents();
        }

        public override void UpdateState() {
            // Logica de verifica��o se acertou algo no caminho:
            //if (Physics.BoxCast(, 
            //    , _player.transform.forward, out RaycastHit hit, _player.transform.rotation, _player.BaseStatsSO.meleeRange)) {

            //    // Se for bullet : aplica parry,
            //    if (hit.collider.TryGetComponent(out Bullet bullet)) {
            //        bullet.Parry(_player);
            //    // Se for player : aplica knockback;
            //    } else if (hit.collider.TryGetComponent(out PlayerHit player)) {
            //        Debug.Log($"Acertei o player '{hit.collider.gameObject.name}' a uma distância de {hit.distance}");
            //        player.PlayerKnockback(_player);
            //    }
            //}
        }

        protected override void HandleShoot(object sender, EventArgs e) {
            // Previne atirar durante um melee.
        }

        protected override void HandleMelee(object sender, EventArgs e) {
            // Previne ataque melee durante um melee.
        }

        protected override void HandleDash(object sender, EventArgs e) {
            // Previne de dar outro dashe antes do t�rmino de um.
        }

        private void WhenMeleeEnds() {
            _player.TransitionToState(_player.StateFactory.GetState(StateFactory.StateType.Idle));
        }
    }
}
