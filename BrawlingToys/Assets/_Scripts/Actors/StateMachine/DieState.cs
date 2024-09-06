using System;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class DieState : State
    {
        protected override void EnterState()
        {
            _player._inputs.DisablePlayerMap();
        }

        protected override void ExitState()
        {

        }

        protected override void HandleShoot(object sender, EventArgs e)
        {
            // Previne de atirar durante a morte.
        }

        protected override void HandleMelee(object sender, EventArgs e)
        {
            // Previne ataque melee durante a morte.
        }

        protected override void HandleDash(object sender, EventArgs e)
        {
            // Previne dar dash
        }

        public override void HandleDie()
        {
            // Previne de entrar no estado de morte novamente
        }
    }
}
