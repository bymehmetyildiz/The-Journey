using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerState
{    
    private bool isGrounded;
    private bool isTouchingCeiling;
    

    public PlayerDodgeState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckGround();
        isTouchingCeiling = player.CheckCeiling();
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocityY(0);
        player.SetVelocityX(player.facingDirection * playerData.movementVelocity * 2);
        Physics2D.IgnoreLayerCollision(player.playerLayer, player.enemyLayer, true);
    }

    public override void Exit()
    {
        base.Exit();
        Physics2D.IgnoreLayerCollision(player.playerLayer, player.enemyLayer, false);

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();        

        player.SetVelocityY(0);
        player.SetVelocityX(player.facingDirection * playerData.movementVelocity * 2);

        if (isAnimationFinished && !isGrounded)
        {
            stateMachine.ChangeState(player.inAirState);
        }
        else if(isAnimationFinished && isGrounded)
        {
            stateMachine.ChangeState(player.idleState);
        }
        else if (isAnimationFinished && isTouchingCeiling)
        {
            stateMachine.ChangeState(player.crouchIdleState);
        }


    }

}
