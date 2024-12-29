using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArcherState : PlayerAbilityState
{
    protected int xInput;
    protected D_RangedAttackState stateData;
    protected GameObject projectile;
    protected Projectile projectileScript;
    public PlayerArcherState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        projectile = GameObject.Instantiate(playerData.projectile, player.arrowPosition.position, player.arrowPosition.rotation);
        projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.FireProjectile(playerData.projectileSpeed, playerData.projectileTravelDistance, playerData.projectileDamage);
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
        
    }

    public override void Exit()
    {
        base.Exit();
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.inputHandler.normInputX;

        player.anim.SetBool("Hold", player.inputHandler.attackInputs[(int)(CombatInputs.secondary)]);

        player.SetZeroVelocity();
        player.FlipController(xInput);

        if (isAnimationFinished)
        {           
            stateMachine.ChangeState(player.idleState);
        }



    }
}
