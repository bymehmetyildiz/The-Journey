using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditStunState : StunState
{
    private Bandit bandit;

    public BanditStunState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_StunState _stateData, Bandit _bandit) : base(_entity, _stateMachine, _animBoolName, _stateData)
    {
        this.bandit = _bandit;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isStunTimeOver)
        {
            if (performCloseRangeAction)
            {
                stateMachine.ChangeState(bandit.meleeAttackState);
            }
            else if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(bandit.chargeState);
            }
            else
            {
                bandit.lookForPlayerState.SetTurnImmediately(true);
                stateMachine.ChangeState(bandit.lookForPlayerState);
            }



        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
