using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherIdleState : IdleState
{
    private Archer archer;

    public ArcherIdleState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_IdleState _stateData, Archer _archer) : base(_entity, _stateMachine, _animBoolName, _stateData)
    {
        this.archer = _archer;
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

        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(archer.playerDetectedState);
        }
        else if (isIdleTimeOver)
        {
            stateMachine.ChangeState(archer.moveState);
            
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
