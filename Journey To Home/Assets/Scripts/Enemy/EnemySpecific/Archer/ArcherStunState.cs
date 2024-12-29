using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStunState : StunState
{
    private Archer archer;

    public ArcherStunState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_StunState _stateData, Archer _archer) : base(_entity, _stateMachine, _animBoolName, _stateData)
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

        if (isStunTimeOver)
        {
            if(isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(archer.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(archer.lookForPlayerState);
            }

        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
