using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherDodgeState : DodgeState
{
    private Archer archer;

    public ArcherDodgeState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_DodgeState _stateData, Archer _archer) : base(_entity, _stateMachine, _animBoolName, _stateData)
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

        if(isDodgeOver) 
        {
            if (isPlayerInMaxAgroRange && performCloseRangeAction)
            {
                stateMachine.ChangeState(archer.meleeAttackState);
            }
            else if(isPlayerInMaxAgroRange && !performCloseRangeAction)
            {
                stateMachine.ChangeState(archer.rangedAttackState);
            }
            else if (!isPlayerInMaxAgroRange)
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
