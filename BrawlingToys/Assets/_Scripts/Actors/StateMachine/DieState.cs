using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : State
{
    protected override void EnterState()
    {

    }

    protected override void ExitState()
    {

    }

    protected override void HandleAttack(object sender, EventArgs e)
    {
        // Previne atacar
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
