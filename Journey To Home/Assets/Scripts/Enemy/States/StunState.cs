using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    public D_StunState stateData;

    protected bool isStunTimeOver;
    protected bool checkGround;
    protected bool isMovementStopped;
    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAgroRange;


    public StunState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_StunState _stateData) : base(_entity, _stateMachine, _animBoolName)
    {
        this.stateData = _stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        checkGround = entity.CheckGround();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        isStunTimeOver = false;
        isMovementStopped = false;         
    }

    public override void Exit()
    {
        base.Exit();
        entity.ResetStunResistance();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time > startTime + stateData.stunTime)
        {
            isStunTimeOver = true;
        }
        if(checkGround && Time.time >= startTime + stateData.stunKnockBackTime && !isMovementStopped) 
        {
            isMovementStopped = true;
            entity.SetVelocity(0);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
