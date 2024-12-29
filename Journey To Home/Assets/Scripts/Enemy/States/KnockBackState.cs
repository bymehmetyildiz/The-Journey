using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackState : State
{
    protected D_KnockBackState stateData;

    public KnockBackState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_KnockBackState _stateData) : base(_entity, _stateMachine, _animBoolName)
    {
        this.stateData = _stateData;
    }

    public override void Enter()
    {
        base.Enter();

        stateData.knockBackTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}
    
