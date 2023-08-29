using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeGoblin : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float collisionDamage = 10f;

    [Header("Attack")]
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] internal float attackDamage = 15f;
   
    [Header("Behavior")]
    [SerializeField] private float seekDistance = 10f;
    [SerializeField] private float moveSpeed = 7f;

    [Header("Canvas")]
    [SerializeField] private RectTransform rectTransform;

    private Transform playerTransform;
    private bool attacking;

    private Animator animator;
    private Rigidbody2D rb;
    

    private enum Animations
    {
        Idle,
        Run,
        Attack
    };

    private enum EnemyStates
    {
        Idle,
        Seeking,
        Attacking
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if (attacking)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackDistance)
        {
            Attack();
        }
        else if (distanceToPlayer <= seekDistance) {

            ChasePlayer();
            FlipScale(playerTransform.position);
        }
        else
        {
            animator.SetInteger("currentAnimation", (int)Animations.Idle);
            rb.velocity = Vector2.zero;
        }
    }

    private void Attack()
    {
        attacking = true;
        animator.SetInteger("currentAnimation", (int)Animations.Attack);
        rb.velocity = Vector2.zero;
    }

    private void FinishAttack()
    {
        attacking = false;
    }

    private void ChasePlayer()
    {
        animator.SetInteger("currentAnimation", (int)Animations.Run);

        Vector2 direction = (transform.position - playerTransform.position).normalized;

        rb.velocity = new Vector2(-direction.x * moveSpeed, rb.velocity.y);
    }

    private void FlipScale(Vector3 target)
    {
        Vector3 scale = transform.localScale;

        if (target.x < transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x) * -1;
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
        }

        rectTransform.localScale = -scale;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();

            if (iDamageable != null)
            {
                //Damage enemy
                iDamageable.TakeDamage(collisionDamage, transform.position);
            }
        }
    }
}
