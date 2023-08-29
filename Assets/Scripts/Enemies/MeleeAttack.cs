using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float collisionDamage = 10f;

    [Header("Attack")]
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] internal float attackDamage = 15f;
    [SerializeField] private bool multipleAttackAnimations;
   
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
        Attack,
        Attack2
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
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        
        if (attacking)
        {
            return;
        }


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
        if (multipleAttackAnimations)
        {
            int randomAnimation = Random.Range(0, 2);

            animator.SetInteger("currentAnimation", randomAnimation == 0 ? (int)Animations.Attack : (int)Animations.Attack2);
        }
        else
        {
            animator.SetInteger("currentAnimation", (int)Animations.Attack);
        }

        attacking = true;
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
