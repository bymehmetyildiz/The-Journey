using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    public int maxHealth = 100;

    [Header("KnockBack State")]
    public Vector2 knockBackSpeed;
    public int lastDamageDirection;

    [Header("Move State")]
    public float movementVelocity = 10f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;

    [Header("Wall Jump State")]
    public float wallJumpVelocity = 20;
    public float walljumpTime = 0.4f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);

    [Header("In Air State")]
    public float coyoteTime;
    public float jumpHeightMultiplier = 0.5f;

    [Header("Wall Slide State")]
    public float wallSlideVelocity = 3f;

    [Header("Wall Climb State")]
    public float wallClimbVelocity = 3f;

    [Header("Ledge Climb State")]
    public Vector2 startOffset;
    public Vector2 stopOffset;

    [Header("Crocuh States")]
    public float crouchMovementVelocity = 5f;
    public float crouchColliderHeight = 0.9f;
    public float standColliderHeight = 1.2f;

    [Header("Attack State")]
    public int damage = 10;
    public int stunDamage = 1;

    [Header("Archer State")]
    public GameObject projectile;
    public float projectileDamage = 10f;
    public float projectileSpeed = 20f;
    public float projectileTravelDistance = 5f;

    [Header("Block State")]
    public GameObject hitPartciles;

    [Header("Check Variables")]
    public float groundCheckRadius = 0.3f;
    public float wallCheckDistance = 0.5f;
    public float attackCheckRadius = 0.5f;
    public LayerMask whatIsGround;
    public LayerMask whatIsEnemy;
    public LayerMask whatIsPlayer;
 
}
