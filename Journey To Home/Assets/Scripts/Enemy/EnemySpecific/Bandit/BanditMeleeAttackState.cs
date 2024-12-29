using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditMeleeAttackState : MeleeAttackState
{
    private Bandit bandit;

    public BanditMeleeAttackState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, Transform _attackPosition, D_MeleeAttack _stateData, Bandit _bandit) : base(_entity, _stateMachine, _animBoolName, _attackPosition, _stateData)
    {
        this.bandit = _bandit;
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

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAnimationOver)
        {
            if(isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(bandit.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(bandit.lookForPlayerState);
            }

        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatIsPlayer);

        foreach (Collider2D collider in detectedObjects)
        {
            Player player = collider.GetComponent<Player>();

            if (player.stateMachine.currentState == player.blockState)
            {
                stateMachine.ChangeState(bandit.stunState);
            }

        }
    }
}
