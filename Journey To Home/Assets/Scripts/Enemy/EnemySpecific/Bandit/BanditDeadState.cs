using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditDeadState : DeadState
{
    private Bandit bandit;

    public BanditDeadState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_DeadState _stateData, Bandit _bandit) : base(_entity, _stateMachine, _animBoolName, _stateData)
    {
        this.bandit = _bandit;
    }

    public override void Enter()
    {
        base.Enter();

        entity.StartCoroutine(Deactivate());
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(2);
        entity.gameObject.SetActive(false);
    }
}
