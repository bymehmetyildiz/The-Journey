using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPos;
    public Vector2 cornerPos;
    private Vector2 startPos, stopPos;

    private bool isHanging;
    private bool isClimbing;
    private bool checkCeiling;
    private bool jumpInput;
    private int xInput;
    private int yInput;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        player.anim.SetBool("Climb", false);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        isHanging = true;
        
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
        player.transform.position = detectedPos;
        cornerPos = player.DetermineCornerPosition();

        startPos.Set(cornerPos.x - (player.facingDirection * playerData.startOffset.x), cornerPos.y - playerData.startOffset.y);
        stopPos.Set(cornerPos.x + (player.facingDirection * playerData.stopOffset.x), cornerPos.y + playerData.stopOffset.y);

        player.transform.position = startPos;     

    }

    public override void Exit()
    {
        base.Exit();

        isHanging = false;

        if (isClimbing)
        {            
            isClimbing = false;
            player.transform.position = stopPos;
        }       

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if (checkCeiling)
            {
                stateMachine.ChangeState(player.crouchIdleState);
            }
            else
            {
                stateMachine.ChangeState(player.idleState);
            }
        }
        else
        {
            xInput = player.inputHandler.normInputX;
            yInput = player.inputHandler.normInputY;
            jumpInput = player.inputHandler.jumpInput;

            player.SetZeroVelocity();
            player.transform.position = startPos;

            if (xInput == player.facingDirection && isHanging && !isClimbing)
            {
                CheckCeiling();
                isClimbing = true;
                player.anim.SetBool("Climb", true);
            }
            else if (yInput == -1 && isHanging && !isClimbing)
            {
                stateMachine.ChangeState(player.inAirState);
            }
            else if (jumpInput && !isClimbing)
            {
                player.wallJumpState.WallJumpDirection(true);
                stateMachine.ChangeState(player.wallJumpState);
            }

        }
    }

    public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;

    private void CheckCeiling()
    {
        checkCeiling = Physics2D.Raycast(cornerPos + (Vector2.up * 0.015f) + (Vector2.right * player.facingDirection * 0.015f), 
           Vector2.up, playerData.standColliderHeight, playerData.whatIsGround);
    }

}
