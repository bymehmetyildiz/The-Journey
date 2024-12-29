using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : Entity
{
    public BanditIdleState idleState { get; private set; }
    public BanditMoveState moveState { get; private set; }
    public BanditPlayerDetectedState playerDetectedState { get; private set; }
    public BanditChargeState chargeState { get; private set; }
    public BanditLookForPlayerState lookForPlayerState { get; private set; }
    public BanditMeleeAttackState meleeAttackState { get; private set; }
    public BanditStunState stunState { get; private set; }
    public BanditDeadState deadState { get; private set; }
    public BanditKnockBackState knockBackState { get; private set;}


    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetected playerDetectedStateData;
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_LookForPlayer lookForPlayerStateData;
    [SerializeField] private D_MeleeAttack meleeAttackStateData;
    [SerializeField] private D_StunState stunStateData;
    [SerializeField] private D_DeadState deadStateData;
    [SerializeField] private D_KnockBackState knockBackStateData;

    [SerializeField] private Transform meleeAttackPosition;

    public override void Start()
    {
        base.Start();

        moveState = new BanditMoveState(this, stateMachine, "Move", moveStateData, this);
        idleState = new BanditIdleState(this, stateMachine, "Idle", idleStateData, this);
        playerDetectedState = new BanditPlayerDetectedState(this, stateMachine,"PlayerDetected", playerDetectedStateData,this);
        chargeState = new BanditChargeState(this, stateMachine,"Charge",chargeStateData, this);
        lookForPlayerState = new BanditLookForPlayerState(this, stateMachine,"LookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new BanditMeleeAttackState(this, stateMachine,"MeleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        stunState = new BanditStunState(this, stateMachine,"Stun",stunStateData, this);
        deadState = new BanditDeadState(this, stateMachine,"Dead", deadStateData, this);
        knockBackState = new BanditKnockBackState(this, stateMachine, "KnockBack", knockBackStateData, this);

        stateMachine.Initialize(moveState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }

    public override void TakeDamage(AttackDetails attackDetails)
    {
        base.TakeDamage(attackDetails);



        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        } 
        else if (isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
        else if(stateMachine.currentState != knockBackState && stateMachine.currentState != meleeAttackState)
        {
            stateMachine.ChangeState(knockBackState);
        }

    }
    
}
