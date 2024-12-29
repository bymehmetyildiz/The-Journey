using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Saw : MonoBehaviour
{
    private AttackDetails attackDetails;

    [SerializeField] private Transform posA, posB;
    [SerializeField] private int moveSpeed;

    private Transform targetPosition;
    private BoxCollider2D bc;

    private int damageAmount = 10;

    void Start()
    {
        targetPosition = posA;
        bc = GetComponent<BoxCollider2D>();

        attackDetails.damageAmount = damageAmount;
        attackDetails.position = transform.position;
    }

    
    void Update()
    {
        attackDetails.position = transform.position;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
        {            
            targetPosition = (targetPosition == posA) ? posB : posA;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.transform.SendMessage("TakeDamage", attackDetails);
        }

    }

}
