using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherMoveState : MoveState
{
    private Archer archer;

    public ArcherMoveState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_MoveState _stateData, Archer _archer) : base(_entity, _stateMachine, _animBoolName, _stateData)
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

        if(isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(archer.playerDetectedState);
        }

        else if (isDetectingWall || !isDetectingLedge)
        {
            archer.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(archer.idleState);
        }


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
