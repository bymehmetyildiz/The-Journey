using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected int yInput;
    private bool jumpInput;
    private bool grabInput;
    private bool dodgeInput;
    private bool isGrounded;
    private bool isTouchingWall;
    protected bool isTouchingCeiling;

    


    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckGround();
        isTouchingWall = player.CheckWall();
        isTouchingCeiling = player.CheckCeiling();
    }

    public override void Enter()
    {
        base.Enter();

        player.jumpState.ResetAmountOfJumpsLeft();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.inputHandler.normInputX;
        yInput = player.inputHandler.normInputY;
        jumpInput = player.inputHandler.jumpInput;
        grabInput = player.inputHandler.grabInput;
        dodgeInput = player.inputHandler.dodgeInput;

        if (player.inputHandler.attackInputs[(int)CombatInputs.primary] && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.attackState);
        }
        else if (player.inputHandler.attackInputs[(int)(CombatInputs.secondary)] && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.archerState);
        }
        else if(player.inputHandler.blockInput && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.blockState);
        }
        else if(jumpInput && player.jumpState.CanJump() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.jumpState);
        }
        else if (!isGrounded)
        {
            player.inAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.inAirState);
        }
        else if (isTouchingWall && grabInput && !isGrounded)
        {
            stateMachine.ChangeState(player.wallGrabState);
        }
        else if (dodgeInput && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.dodgeState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
