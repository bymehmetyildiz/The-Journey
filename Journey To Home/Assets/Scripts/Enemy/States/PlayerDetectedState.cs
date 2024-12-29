using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : State
{
    protected D_PlayerDetected stateData;

    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool performLongRangeAction;
    protected bool performCloseRangeAction;
    protected bool isLedgeDetected;

    public PlayerDetectedState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_PlayerDetected _stateData) : base(_entity, _stateMachine, _animBoolName)
    {
        this.stateData = _stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isLedgeDetected = entity.CheckLedge();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
    }

    public override void Enter()
    {
        base.Enter();
        
        performLongRangeAction = false;
        entity.SetVelocity(0);
      
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + stateData.longRangeActionTime)
        {
            performLongRangeAction = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}