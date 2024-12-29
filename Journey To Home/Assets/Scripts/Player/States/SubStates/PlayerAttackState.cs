using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerAttackState : PlayerAbilityState
{
    private int comboCounter;

    private float comboDuration = 2f;
    private float comboTime;

    private bool isGrounded;

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.SetZeroVelocity();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        player.GiveDamage();
        player.SetVelocityX(playerData.movementVelocity * 0.2f * player.facingDirection);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(player.attackCheck.position, playerData.attackCheckRadius, playerData.whatIsEnemy);

        player.attackDetails.damageAmount = playerData.damage;
        player.attackDetails.position = player.transform.position;
        player.attackDetails.stunDamageAmount = playerData.stunDamage;

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.transform.SendMessage("TakeDamage", player.attackDetails);
        }
    }

    

public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckGround();
    }

    public override void Enter()
    {
        base.Enter();

        player.inputHandler.UseAttackInput();

        player.SetZeroVelocity();

        if (comboCounter > 2 || Time.time >= comboDuration + comboTime)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);

       
    }

    public override void Exit()
    {
        base.Exit();

        comboCounter++;
        comboTime = Time.time;

        player.SetZeroVelocity();
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();        

        if (isAnimationFinished && stateMachine.currentState == player.attackState)
        {
            stateMachine.ChangeState(player.idleState);
        }
        else if (!isGrounded)
        {
            stateMachine.ChangeState(player.inAirState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
