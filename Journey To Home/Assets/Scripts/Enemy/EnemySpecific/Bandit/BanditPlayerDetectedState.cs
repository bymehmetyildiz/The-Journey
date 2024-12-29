using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditPlayerDetectedState : PlayerDetectedState
{
    private Bandit bandit;

    public BanditPlayerDetectedState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_PlayerDetected _stateData, Bandit _bandit) : base(_entity, _stateMachine, _animBoolName, _stateData)
    {
        this.bandit = _bandit;
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

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(bandit.meleeAttackState);
        }
        else if (performLongRangeAction)
        {
            stateMachine.ChangeState(bandit.chargeState);
        }
        else if (!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(bandit.lookForPlayerState);
        }
        else if(!isLedgeDetected) 
        {
            entity.Flip();
            stateMachine.ChangeState(bandit.moveState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
