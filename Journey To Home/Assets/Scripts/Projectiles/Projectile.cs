using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private AttackDetails attackDetails;

    private float speed;
    private float travelDistance;
    private float xStartPos;
    [SerializeField] private float gravity, damageRadius;

    private Rigidbody2D rb;

    private bool isGravityOn;
    private bool hasHitGround;

    [SerializeField] private LayerMask whatIsGround, whatIsPlayer, whatIsEnemy;
    [SerializeField] private Transform damagePosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0.0f;
        rb.velocity =  transform.right * speed;
        

        xStartPos = transform.position.x;

        isGravityOn = false;
    }

    private void Update()
    {
        if (!hasHitGround)
        {
            attackDetails.position = transform.position;

            if(isGravityOn)
            {
                float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

    }

    void FixedUpdate()
    {
        if (!hasHitGround)
        {
            Collider2D damageHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsPlayer);
            Collider2D groundHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsGround);
            Collider2D enemyHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsEnemy);

            if (damageHit)
            {
                damageHit.transform.SendMessage("TakeDamage", attackDetails);
                Destroy(gameObject);
            }

            if (enemyHit)
            {
                enemyHit.transform.SendMessage("TakeDamage", attackDetails);
                Destroy(gameObject);
            }

            if (groundHit)
            {
                hasHitGround = true;
                rb.gravityScale = 0.0f;
                rb.velocity = Vector2.zero;
                Destroy(gameObject, 5f);
            }


            if(Mathf.Abs(xStartPos - transform.position.x) >= travelDistance && !isGravityOn)
            {
                isGravityOn = true;
                rb.gravityScale = gravity;
            }

        }


    }

    public void FireProjectile(float _speed, float _travelDistance, float _damage)
    {
        speed = _speed;
        travelDistance = _travelDistance;
        attackDetails.damageAmount = _damage;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    }
}
