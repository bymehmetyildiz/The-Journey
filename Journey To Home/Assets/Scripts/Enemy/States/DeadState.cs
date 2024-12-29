using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    protected D_DeadState stateData;
    protected bool checkGround;
    public DeadState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_DeadState _stateData) : base(_entity, _stateMachine, _animBoolName)
    {
        this.stateData = _stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        checkGround = entity.CheckGround();
        
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(stateData.knockBackSpeed, stateData.knockbackAngle, entity.lastDamageDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (checkGround && Time.time >= startTime + stateData.knockBackTime )
        {            
            entity.SetVelocity(0);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
