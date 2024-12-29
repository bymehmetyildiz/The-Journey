using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }


    [Header("Move Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    private float xInput;
    private float yInput;
    private bool facingRight = true;
    private bool canMove = true;

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private LayerMask WhatIsObject;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform wallCheck;

    [Header("Wall Jump/Slide Info")]
    private bool canWallSlide;
    private bool isWallSliding;
    private bool wallJump;
    public int facingDirection = 1;

    [Header("Attack Info")]
    [SerializeField] private float comboTime;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask WhatIsEnemy;
    private float comboTimeCounter;
    private bool isAttacking;
    private int comboCounter;

    [Header("Roll Info")]
    [SerializeField] private float rollDuration;
    private float rollTime;
    [SerializeField] private float rollSpeed;
    [SerializeField] private float rollCoolDown;
    private float rollCoolDownTimer;
    private bool isRolling;

    [Header("Archer Info")]
    private bool isHolding;
    private float arrowTimer;
    private float arrowCooldown = 0.3f;
    private int arrowQuantity;

    [Header("Knockback Info")]
    [SerializeField] private float knockbackDuration;
    [SerializeField] Vector2 knockbackSpeed;
    private bool knockback;
    private float knockbackStartTime;

    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D cc;
    private PlayerStats ps;


    private AttackDetails attackDetails;
    [SerializeField] private float damage = 30;
    [SerializeField] private float stunDamage = 1f;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
        
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        cc = GetComponent<CapsuleCollider2D>();
        ps = GetComponent<PlayerStats>();
    }

    
    void Update()
    {

        Movement();
        Jump();
        FlipController();
        AnimatorControllers();
        Attack();
        Roll();
        Archery();
        CheckKnockback();
        
    }

    #region States
    private void Movement()
    {
        if (!knockback && !ps.dead)
        {
            if (!isGrounded() && rb.velocity.y < 0)
            {
                canWallSlide = true;
            }

            if (canMove)
            {

                xInput = Input.GetAxisRaw("Horizontal");
                yInput = Input.GetAxisRaw("Vertical");

                if (rollTime > 0)
                {
                    rb.velocity = new Vector2(facingDirection * rollSpeed, 0);
                    xInput = 0;
                }

                else
                {
                    WallSlide();
                }

            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }

        
        
    }
    private void WallSlide()
    {
        if (!knockback && !ps.dead)
        {
            if (isWallDetected() && canWallSlide && rb.velocity.y < 0)
            {

                isWallSliding = true;

                if (yInput < 0)
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
                else
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.7f);

            }
            else // Movement
            {
                isWallSliding = false;
                rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);

            }

            if (isGrounded())
                isWallSliding = false;
        }


        
    }
    private bool isMoving()
    {
        if (xInput != 0)
            return true;

        return false;
    }
    private void Jump()
    {
        if (!knockback && !ps.dead)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded())
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                }
                else if (!isGrounded() && isWallSliding)
                {
                    Flip();
                    rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                }
            }
        }

    
    }     
    private void Attack()
    {
        comboTimeCounter -= Time.deltaTime;

        if (comboTimeCounter < 0)
            comboCounter = 0;

        if (!knockback && !ps.dead)
        {                   

            if (Input.GetKeyDown(KeyCode.Mouse0) && isGrounded())
            {
                comboTimeCounter = comboTime;
                isAttacking = true;
                canMove = false;

            }
        }

            
    }
    public void AttackOver()
    {
        isAttacking = false;

        comboCounter++;

        if (comboCounter > 2)
            comboCounter = 0;

        canMove = true;
    }
    private void Roll()
    {
        rollTime -= Time.deltaTime;
        rollCoolDownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && rollCoolDownTimer < 0)
        {
            rollCoolDownTimer = rollCoolDown;
            rollTime = rollDuration;            
        }
            
    }
    private void Archery()
    {
        arrowTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse1) && isGrounded() && arrowTimer < 0 && arrowQuantity > 0)
        {
            isAttacking = false;
            canMove = false;
            isHolding = true;            
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            canMove = true;
            isHolding = false;
            arrowTimer = arrowCooldown;
            if(arrowQuantity > 0)
                arrowQuantity--;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Arrow")
        {
            arrowQuantity += 25;            
        }

    }

    public void SwordDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, WhatIsEnemy);
        attackDetails.damageAmount = damage;
        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = stunDamage;

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.transform.SendMessage("Damage", attackDetails);
        }

    }

    public void ArrowDamage()
    {
        Debug.Log("Arrow Damage");
    }

    private void KnockBack(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);        
       
    }

    private void CheckKnockback()
    {
        if (Time.time > knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
            
        }

    }

   

    private void Damage(AttackDetails attackDetails)
    {
        if (rollTime < 0 && !ps.dead )
        {
            int direction;

            ps.TakeDamage(attackDetails.damageAmount);

            if (attackDetails.position.x < transform.position.x)
                direction = 1;
            else
                direction = -1;

            KnockBack(direction);
        }       

    }


    private bool isGrounded() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, WhatIsGround);
    private bool isWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, WhatIsGround);
    #endregion


    private void AnimatorControllers()
    {
        anim.SetBool("isMoving", isMoving());
        anim.SetBool("isGrounded", isGrounded());
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter", comboCounter);
        anim.SetBool("isRolling", rollTime > 0);
        anim.SetBool("isWallSliding", isWallSliding);  
        anim.SetBool("isHolding", isHolding);     
        
    }

    #region Flip
    private void FlipController()
    {
        if (xInput > 0 && !facingRight)
            Flip();
        if (xInput < 0 && facingRight)
            Flip();
    }

    private void Flip()
    {
        if (!knockback)
        {
            facingDirection = facingDirection * (-1);
            //Vector3 currentScale = transform.localScale;
            //currentScale.x *= -1;
            //transform.localScale = currentScale;
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }

       
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);

    }
    
  

}
