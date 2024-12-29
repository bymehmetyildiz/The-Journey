using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditLookForPlayerState : LookForPlayerState
{
    public Bandit bandit;

    public BanditLookForPlayerState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_LookForPlayer _stateData, Bandit _bandit) : base(_entity, _stateMachine, _animBoolName, _stateData)
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

        if (isPlayerInMinagroRange)
        {
            stateMachine.ChangeState(bandit.playerDetectedState);
        }
        else if (isAllTurnsTimeDone)
        {
            stateMachine.ChangeState(bandit.moveState);
        }


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
