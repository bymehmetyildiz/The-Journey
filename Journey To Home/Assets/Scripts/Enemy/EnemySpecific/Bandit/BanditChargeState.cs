using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditChargeState : ChargeState
{
    private Bandit bandit;

    public BanditChargeState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_ChargeState _stateData, Bandit _bandit) : base(_entity, _stateMachine, _animBoolName, _stateData)
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

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(bandit.meleeAttackState);
        }

        else if (!isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(bandit.lookForPlayerState);
        }

        else if(isChargeTimeOver)
        {
            if(isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(bandit.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(bandit.lookForPlayerState);
            }

        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
