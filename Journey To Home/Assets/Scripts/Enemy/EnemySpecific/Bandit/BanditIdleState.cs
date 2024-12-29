using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditIdleState : IdleState
{
    private Bandit bandit;
    
    public BanditIdleState( Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_IdleState _stateData, Bandit _bandit) : base(_entity, _stateMachine,  _animBoolName, _stateData)
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

        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(bandit.playerDetectedState);
        }

        else if (isIdleTimeOver)
        {
            stateMachine.ChangeState(bandit.moveState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
