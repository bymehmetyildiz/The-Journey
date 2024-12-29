using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockBackState : PlayerState
{
    private bool isGrounded;

    public PlayerKnockBackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckGround();
    }

    public override void Enter()
    {
        base.Enter();

        player.rb.velocity = new Vector2(playerData.knockBackSpeed.x * playerData.lastDamageDirection, playerData.knockBackSpeed.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();        

        if(isAnimationFinished)
        {
            if(isGrounded)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                stateMachine.ChangeState(player.inAirState);
            }

        }
    }
}
