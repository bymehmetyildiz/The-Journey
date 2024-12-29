using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerInAirState inAirState { get; private set; }
    public PlayerLandState landState { get; private set; }
    public PlayerWallClimbState wallClimbState { get; private set; }
    public PlayerWallGrabState wallGrabState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerLedgeClimbState ledgeClimbState { get; private set; }
    public PlayerCrouchIdleState crouchIdleState { get; private set; }
    public PlayerCrouchMoveState crouchMoveState { get; private set; }
    public PlayerAttackState attackState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    public PlayerArcherState archerState { get; private set; }
    public PlayerBlockState blockState { get; private set; }
    public PlayerDodgeState dodgeState { get; private set; }
    public PlayerKnockBackState knockBackState { get; private set; }
    #endregion

    #region Components
    public Animator anim { get; private set; }
    public PlayerInputHandler inputHandler { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sp { get; private set; }
    public BoxCollider2D boxCollider { get; private set; }
    #endregion

    #region Check Transforms

    [SerializeField] private Transform groundCheck, wallCheck, ledgeCheck, ceilingCheck;
    public Transform attackCheck;
    [SerializeField] private PlayerData playerData;

    #endregion

    #region Other Variables
    public int facingDirection { get; private set; }

    private float currentHealth;
    public Vector2 currentVelocity { get; private set; }   

    public Transform arrowPosition;

    private Vector2 workSpace;

    public AttackDetails attackDetails;    

    public int playerLayer  {get;private set;}
    public int enemyLayer { get;private set;}
    #endregion

    #region UI Elements
    [SerializeField] private Image healthBar;

    #endregion

    #region Unity Functions
    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        
        idleState = new PlayerIdleState(this, stateMachine, playerData, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, playerData, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, playerData, "InAir");
        inAirState = new PlayerInAirState(this, stateMachine, playerData, "InAir");
        landState = new PlayerLandState(this, stateMachine, playerData, "Land");
        wallClimbState = new PlayerWallClimbState(this, stateMachine, playerData, "WallClimb");
        wallGrabState = new PlayerWallGrabState(this, stateMachine, playerData, "WallGrab");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, playerData, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, playerData, "InAir");
        ledgeClimbState = new PlayerLedgeClimbState(this, stateMachine, playerData, "LedgeClimb");
        crouchIdleState = new PlayerCrouchIdleState(this, stateMachine, playerData, "CrouchIdle");
        crouchMoveState = new PlayerCrouchMoveState(this, stateMachine, playerData, "CrouchMove");
        attackState = new PlayerAttackState(this, stateMachine, playerData, "Attack");
        archerState = new PlayerArcherState(this, stateMachine, playerData, "Archer");
        blockState = new PlayerBlockState(this, stateMachine, playerData, "Block");
        dodgeState = new PlayerDodgeState(this, stateMachine, playerData, "Dodge");
        deadState = new PlayerDeadState(this, stateMachine, playerData, "Dead");
        knockBackState = new PlayerKnockBackState(this, stateMachine, playerData, "KnockBack");
    }

    void Start()
    {
        anim = GetComponent<Animator>();

        inputHandler = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        currentHealth = playerData.maxHealth;

        stateMachine.Initialize(idleState);
        facingDirection = 1;

        playerLayer = this.gameObject.layer;
        enemyLayer = LayerMask.NameToLayer("Enemy");

        healthBar.fillAmount = currentHealth/playerData.maxHealth;
        
    }
    
    void Update()
    {        
        currentVelocity = rb.velocity;        
        stateMachine.currentState.LogicUpdate();

        healthBar.fillAmount = currentHealth / playerData.maxHealth;
    }

    private void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }
    #endregion

    #region Set Functions
    public void SetZeroVelocity()
    {
        rb.velocity = Vector2.zero;
        currentVelocity = Vector2.zero;
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workSpace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = workSpace;
        currentVelocity = workSpace;
    }


    public void SetVelocityX(float velocity)
    {
        workSpace.Set(velocity, currentVelocity.y);
        rb.velocity = workSpace;
        currentVelocity = workSpace;
    }

    public void SetVelocityY(float velocity)
    {
        workSpace.Set(currentVelocity.x, velocity);
        rb.velocity = workSpace;
        currentVelocity = workSpace;
    }
    #endregion

    #region Check Functions
    public bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);        
    }
    public bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }
    public bool CheckWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -facingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }
    public bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.right * facingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }
    public bool CheckCeiling()
    {
        return Physics2D.OverlapCircle(ceilingCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public void GiveDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackCheck.position, playerData.attackCheckRadius, playerData.whatIsEnemy);
        attackDetails.damageAmount = playerData.damage;
        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = playerData.stunDamage;

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.transform.SendMessage("TakeDamage", attackDetails);
        }
    }    

    public void TakeDamage(AttackDetails attackDetails)
    {
        if(stateMachine.currentState != blockState)
        {
            currentHealth -= attackDetails.damageAmount;

            if (attackDetails.position.x >= rb.transform.position.x)
            {
                playerData.lastDamageDirection = -1;
            }
            else if(attackDetails.position.x < rb.transform.position.x)
            {
                playerData.lastDamageDirection = 1;
            }
            
            stateMachine.ChangeState(knockBackState);

            if (currentHealth <= 0)
            {
                stateMachine.ChangeState(deadState);
            }
        }
        else
        {
            Instantiate(playerData.hitPartciles, attackCheck.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }

       

    }

    #endregion

    #region Other Functions
    public void SetColliderHeight(float height)
    {
        Vector2 center = boxCollider.offset;
        workSpace.Set(boxCollider.size.x, height);

        center.y += (height - boxCollider.size.y) / 2;

        boxCollider.size = workSpace;
        boxCollider.offset = center;
    }


    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        float xDist = xHit.distance;
        workSpace.Set((xDist + 0.015f) * facingDirection, 0);
        RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3)(workSpace), Vector2.down, ledgeCheck.position.y - wallCheck.position.y + 0.015f, playerData.whatIsGround);
        float yDist = yHit.distance;

        workSpace.Set(wallCheck.position.x + (xDist * facingDirection), ledgeCheck.position.y - yDist);
        return workSpace;
    }
    #endregion

    #region Flip functions
    public void FlipController(int xInput)
    {
        if(xInput != 0 && xInput != facingDirection)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    #endregion

    #region Animation Triggers
    private void AnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }

    private void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    #endregion

    #region Gizmos Function
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * playerData.wallCheckDistance * facingDirection));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.right * playerData.wallCheckDistance * facingDirection));
        Gizmos.DrawWireSphere(attackCheck.position, playerData.attackCheckRadius);
       
    }
    #endregion
}
