using System.Collections;
using System.IO.IsolatedStorage;
using Unity.VisualScripting;
using UnityEngine;


public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;

    public D_Entity entityData;
    public int facingDirection {  get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }

    [SerializeField] private Transform wallCheck, ledgeCheck, playerCheck, groundCheck, hitParticleCheck;

    private Vector2 velocityWorkSpace;

    private float currentHealth;
    private float currentStunResitance;
    public float lastDamageTime { get; private set; }

    protected bool isStunned;
    protected bool isDead;

    public int lastDamageDirection { get; private set; }

    public virtual void Start()
    {
        facingDirection = 1;
        currentHealth = entityData.maxHealth;
        currentStunResitance = entityData.stunResistance;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        stateMachine = new FiniteStateMachine();
        atsm = GetComponentInChildren<AnimationToStateMachine>();
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();

        anim.SetFloat("yVelocity", rb.velocity.y);

        if(Time.time > lastDamageTime + entityData.stunRecoveryTime)
        {
            ResetStunResistance();
        }
        
    }

    

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float _velocity)
    {
        velocityWorkSpace.Set(facingDirection * _velocity , rb.velocity.y);
        rb.velocity = velocityWorkSpace;
    }

    public virtual void SetVelocity(float _velocity, Vector2 _angle, int _direction)
    {
        _angle.Normalize();
        velocityWorkSpace.Set(_angle.x * _direction * _velocity, _angle.y * _velocity);
        rb.velocity = velocityWorkSpace;
    }


    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, entityData.wallCheckDistance, entityData.whatIsGround);
    }


    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {

        return Physics2D.Raycast(playerCheck.position, Vector2.right * facingDirection, entityData.minAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, Vector2.right * facingDirection, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {      
        return Physics2D.Raycast(playerCheck.position, Vector2.right * facingDirection, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    } 

    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, entityData.groundCheckRadius, entityData.whatIsGround);
    }

    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResitance = entityData.stunResistance;
    }


    public virtual void TakeDamage(AttackDetails attackDetails)
    {
        lastDamageTime = Time.time;

        currentStunResitance -= attackDetails.stunDamageAmount;

        currentHealth -= attackDetails.damageAmount;       

        Instantiate(entityData.hitParticles,hitParticleCheck.position, Quaternion.Euler(0,0,Random.Range(0,360)));

        if (attackDetails.position.x >= rb.transform.position.x)
        {
            lastDamageDirection = -1;
        }
        else if (attackDetails.position.x < rb.transform.position.x)
        {
            lastDamageDirection = 1;
        }       

        if (currentStunResitance <= 0)
        {
            isStunned = true;
        }  
        
        if (currentHealth <= 0)
        {
            isDead = true;
        }                
    }


    public virtual void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position +  (Vector3)(Vector2.down * entityData.ledgeCheckDistance));
        //Gizmos.DrawLine(playerCheck.position, playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.minAgroDistance));  

        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.closeRangeActionDistance),0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.minAgroDistance),0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.maxAgroDistance),0.2f);     
    }



}
