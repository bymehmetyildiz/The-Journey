using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherDeadState : DeadState
{
    private Archer archer;

    public ArcherDeadState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_DeadState _stateData, Archer _archer) : base(_entity, _stateMachine, _animBoolName, _stateData)
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
