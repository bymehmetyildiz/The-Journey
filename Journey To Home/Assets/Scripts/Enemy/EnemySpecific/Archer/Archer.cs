using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Entity
{
    public ArcherMoveState moveState { get; private set; }
    public ArcherIdleState idleState { get; private set; }
    public ArcherPlayerDetectedState playerDetectedState { get; private set; }
    public ArcherMeleeAttackState meleeAttackState { get; private set; }
    public ArcherLookForPlayerState lookForPlayerState { get; private set;}
    public ArcherStunState stunState { get; private set; }
    public ArcherDeadState deadState { get; private set; }
    public ArcherDodgeState dodgeState { get; private set;}
    public ArcherRangedAttackState rangedAttackState { get; private set;}

    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_PlayerDetected playerDetectedStateData;
    [SerializeField] private D_MeleeAttack meleeAttackStateData;
    [SerializeField] private D_LookForPlayer lookForPlayerStateData;
    [SerializeField] private D_StunState stunStateData;
    [SerializeField] private D_DeadState deadStateData;
    [SerializeField] private D_RangedAttackState rangedAttackStateData;
    public D_DodgeState dodgeStateData;

    [SerializeField] private Transform meleeAttackPosition, rangedAttackPosition;

    public override void Start()
    {
        base.Start();

        moveState = new ArcherMoveState(this, stateMachine, "Move", moveStateData, this);
        idleState = new ArcherIdleState(this, stateMachine, "Idle", idleStateData, this);
        playerDetectedState = new ArcherPlayerDetectedState(this, stateMachine,"PlayerDetected", playerDetectedStateData, this);
        meleeAttackState = new ArcherMeleeAttackState(this, stateMachine,"MeleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        lookForPlayerState = new ArcherLookForPlayerState(this, stateMachine, "LookForPlayer", lookForPlayerStateData, this);
        stunState = new ArcherStunState(this, stateMachine, "Stun", stunStateData, this);
        deadState = new ArcherDeadState(this, stateMachine, "Dead", deadStateData, this);
        dodgeState = new ArcherDodgeState(this, stateMachine, "Dodge", dodgeStateData, this);
        rangedAttackState = new ArcherRangedAttackState(this, stateMachine,"RangedAttack", rangedAttackPosition, rangedAttackStateData, this);

        stateMachine.Initialize(moveState);
    }


    public override void TakeDamage(AttackDetails attackDetails)
    {
        base.TakeDamage(attackDetails);

        if(isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else if (isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
        else if(CheckPlayerInMinAgroRange())
        {
            stateMachine.ChangeState(rangedAttackState);
        }
        else if (!CheckPlayerInMinAgroRange())
        {
            lookForPlayerState.SetTurnImmediately(true);
            stateMachine.ChangeState(lookForPlayerState);
        }


    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}
