using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    private enum State
    {   
        Idle,
        Walk,
        Knocback,
        Dead,
        Attack
    }

    private State currentState;
    private bool groundDetected, wallDetected, playerDetected;
    [SerializeField] private Transform groundCheck, wallCheck, touchDamageCheck, player, playerCheck;
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    [SerializeField] private float groundCheckDistance, wallCheckDistance, moveSpeed, maxHealth, knockbackDuration, playerCheckDistance, idleDuration, attackCoolDownDuration;
    [SerializeField] private float touchDamage,touchDamageCooldown, touchDamageWidth, touchDamageHeight, lastTouchDamageTime;
    [SerializeField] private Vector2 knockBackSpeed;
    [SerializeField] private GameObject hitParticles;
    private int facingDirection, damageDirection;
    private float currentHealth, knockbackStartTime, idleStartTime, attackCoolDownStart;
    private float[] attackDetails = new float[2];
    private Vector2 movement, touchDamageBottomLeft, touchDamageTopRight, raycastDirection;
    private Rigidbody2D rb;
    private Animator anim;

    private void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        facingDirection = 1;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                UpdateIdleState(); 
                break;

            case State.Walk:
                UpdateWalkingState(); 
                break;

            case State.Knocback:
                UpdateKnockbackState();
                break;

            case State.Dead:
                UpdateDeadState();
                break;
            case State.Attack: 
                UpdateAttackState(); 
                break;
        }

    }

    //--Idle State -----------------------------------

    private void EnterIdleState()
    {
        idleStartTime = Time.time;
        rb.velocity = Vector2.zero;        
    }

    private void UpdateIdleState()
    {
        CheckTouchDamage();

        if (Time.time > idleStartTime + idleDuration)
            SwitchState(State.Walk);
    }

    private void ExitIdleState()
    {

    }

    //--Walking State --------------------------------

    private void EnterWalkingState()
    {
        anim.SetBool("Run", true);
        Flip();
    }

    private void UpdateWalkingState()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right  , wallCheckDistance  , whatIsGround);
        playerDetected = Physics2D.Raycast(playerCheck.position, Vector2.right, playerCheckDistance , whatIsPlayer);

        CheckTouchDamage();

        if (!groundDetected || wallDetected)
        { 
            SwitchState(State.Idle);
            
        }
        else if(playerDetected)
        {
            if (player.transform.position.x < transform.position.x && facingDirection == 1)
                Flip();
            else if (player.transform.position.x > transform.position.x && facingDirection == -1)
                Flip();

            movement.Set(moveSpeed * facingDirection, rb.velocity.y);
            rb.velocity = movement;            



            if (Mathf.Abs(transform.position.x - player.position.x) <= 1)
            {
                SwitchState(State.Attack);
                Debug.Log(Mathf.Abs(transform.position.x - player.position.x));
            }

        }

        else
        {
            movement.Set(moveSpeed * facingDirection, rb.velocity.y);
            rb.velocity = movement;
        }

    }

    private void ExitWalkingState()
    {
        anim.SetBool("Run", false);
    }

    //--KnockBack State--------------------------------

    private void EnterKnockbackState()
    {
        knockbackStartTime = Time.time;
        movement.Set(knockBackSpeed.x * damageDirection, knockBackSpeed.y);
        rb.velocity = movement;
        anim.SetBool("Knockback", true);
    }

    private void UpdateKnockbackState()
    {
        if (Time.time > knockbackStartTime + knockbackDuration)
        {
            SwitchState(State.Walk);
        }

    }

    private void ExitKnockbackState()
    {
        anim.SetBool("Knockback", false);
    }

    //-- Attack State ----------------------------------

    private void EnterAttackState()
    {
        attackCoolDownStart = Time.time;
        rb.velocity = Vector2.zero;
        anim.SetBool("Attack", true);
    }

    private void UpdateAttackState()
    {
        CheckTouchDamage();

        if (player.transform.position.x < transform.position.x && facingDirection == 1)
            Flip();
        else if(player.transform.position.x > transform.position.x && facingDirection == -1)
            Flip();

        if(Time.time > attackCoolDownStart + attackCoolDownDuration)
            SwitchState(State.Idle);
    }

    private void ExitAttackState()
    {
        anim.SetBool("Attack", false);
    }

    //--Dead State--------------------------------------

    private void EnterDeadState()
    {
        movement.Set(knockBackSpeed.x * damageDirection, knockBackSpeed.y);
        rb.velocity = movement;
        anim.SetBool("Dead", true);
        Destroy(gameObject, 2);
    }

    private void UpdateDeadState()
    {

    }

    private void ExitDeadState()
    {

    }


    //-- Other Functions---------------------------------
    private void Damage(float[] attackDetails)
    {
        currentHealth -= attackDetails[0];
        Instantiate(hitParticles, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

        if (attackDetails[1] > rb.transform.position.x)
        {
            damageDirection = -1;
        }
        else
        {
            damageDirection = 1;
        }

        // HitParticle

        if (currentHealth > 0.0f)
        {
            SwitchState(State.Knocback);
        }
        else if (currentHealth <= 0.0f)
        {
            SwitchState(State.Dead);
        }
    }

    private void CheckTouchDamage()
    {
        if (Time.time >= lastTouchDamageTime + touchDamageCooldown)
        {
            touchDamageBottomLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
            touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

            Collider2D hit = Physics2D.OverlapArea(touchDamageBottomLeft, touchDamageTopRight, whatIsPlayer);

            if (hit != null)
            {
                lastTouchDamageTime = Time.time;
                attackDetails[0] = touchDamage;
                attackDetails[1] = rb.transform.position.x;
                hit.SendMessage("Damage", attackDetails);
            }
        }

    }


    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0, 180, 0);
        
    }

    private void SwitchState(State state)
    {
        switch(currentState)
        {
            case State.Idle:
                ExitIdleState(); 
                break;
            case State.Walk:
                ExitWalkingState();
                break;
            case State.Knocback:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
            case State.Attack:
                ExitAttackState();
                break;
        }

        switch (state)
        {
            case State.Idle:
                EnterIdleState(); 
                break;
            case State.Walk:
                EnterWalkingState();
                break;
            case State.Knocback:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
            case State.Attack:
                EnterAttackState();
                break;
        }

        currentState = state;

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y ));
        Gizmos.DrawLine(playerCheck.position, new Vector2(playerCheck.position.x + playerCheckDistance * facingDirection, playerCheck.position.y ));

        Vector2 botLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 botRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 topRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));
        Vector2 topLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }


}
