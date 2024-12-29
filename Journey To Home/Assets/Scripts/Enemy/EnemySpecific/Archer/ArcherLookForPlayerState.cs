using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherLookForPlayerState : LookForPlayerState
{
    private Archer archer;

    public ArcherLookForPlayerState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_LookForPlayer _stateData, Archer _archer) : base(_entity, _stateMachine, _animBoolName, _stateData)
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

        if (isPlayerInMinagroRange)
        {
            stateMachine.ChangeState(archer.playerDetectedState);
        }
        else if (isAllTurnsTimeDone)
        {
            stateMachine.ChangeState(archer.moveState);
        }



    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
