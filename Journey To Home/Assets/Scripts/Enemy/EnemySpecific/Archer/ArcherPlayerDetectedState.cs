using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherPlayerDetectedState : PlayerDetectedState
{
    private Archer archer;

    public ArcherPlayerDetectedState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_PlayerDetected _stateData, Archer _archer) : base(_entity, _stateMachine, _animBoolName, _stateData)
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
        
        if (performCloseRangeAction)
        {
            if (Time.time >= archer.dodgeState.startTime + archer.dodgeStateData.dodgeCooldown)
            {
                stateMachine.ChangeState(archer.dodgeState);
            }
            else
            {
                stateMachine.ChangeState(archer.meleeAttackState);
            }

        }
        else if( performLongRangeAction)
        {
            stateMachine.ChangeState(archer.rangedAttackState);
        }

        else if (!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(archer.lookForPlayerState);
        }


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
