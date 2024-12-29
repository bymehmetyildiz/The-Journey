using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForPlayerState : State
{
    protected D_LookForPlayer stateData;

    protected bool turnImmediately;
    protected bool isPlayerInMinagroRange;
    protected bool isAllTurnsDone;
    protected bool isAllTurnsTimeDone;

    protected float lastTurnTime;

    protected int amountOfTurnsDone;

    public LookForPlayerState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_LookForPlayer _stateData) : base(_entity, _stateMachine, _animBoolName)
    {
        this.stateData = _stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinagroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        isAllTurnsDone = false;
        isAllTurnsTimeDone = false;

        lastTurnTime = startTime;
        amountOfTurnsDone = 0;

        entity.SetVelocity(0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(turnImmediately)
        {
            entity.Flip();
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
            turnImmediately = false;
        }
        else if(Time.time > lastTurnTime + stateData.timeBetweenTurns && !isAllTurnsDone)
        {
            entity.Flip();
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
        }

        if(amountOfTurnsDone >= stateData.amountOfTurns)
        {
            isAllTurnsDone = true;
        }

        if (Time.time >= lastTurnTime + stateData.timeBetweenTurns && isAllTurnsDone)
        {
            isAllTurnsTimeDone = true;
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetTurnImmediately(bool flip)
    {
        turnImmediately = flip; 
    }

}
