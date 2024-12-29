using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditKnockBackState : KnockBackState
{
    private Bandit bandit;

    public BanditKnockBackState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_KnockBackState _stateData, Bandit _bandit) : base(_entity, _stateMachine, _animBoolName, _stateData)
    {
        this.bandit = _bandit;
    }

    public override void Enter()
    {
        base.Enter();

        entity.rb.velocity = new Vector2(stateData.knockBackSpeed.x * entity.lastDamageDirection, stateData.knockBackSpeed.y);
    }

    public override void Exit()
    {
        base.Exit();        
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= stateData.knockBackTime + stateData.knockBackDuration)
        {
            stateMachine.ChangeState(bandit.lookForPlayerState);
        }

    }
}

