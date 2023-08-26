using System;
using System.Collections;
using UnityEngine;

public class SpiderEnemy : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float damage = 20f;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Canvas")]
    [SerializeField] private RectTransform rectTransform;

    [Header("Behavior")]
    [SerializeField] private float seekDistance = 7f;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float timeToRecoverSpeed = 1f;

    private Vector2 idlePosition;
    private bool attacking = false;
    private float initialSpeed;
    private EnemyStates enemyState;
    private Transform playerTransform;

    private Animator animator;
    private Rigidbody2D rb;
    private EnemyHealth enemyHealth;

    private enum Animations
    {
        Idle,
        Run,
        Attack
    };

    private enum EnemyStates
    {
        ToIdle,
        Idle,
        Seeking,
        Attacking
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Start()
    {
        idlePosition = transform.position;
        initialSpeed = moveSpeed;
    }

    private void Update()
    {
        if (enemyHealth.health <= 0f)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackDistance || attacking)
        {
            enemyState = EnemyStates.Attacking;
        }
        else if (distanceToPlayer <= seekDistance)
        {
            enemyState = EnemyStates.Seeking;
        }
        else
        {
            if (Vector2.Distance(transform.position, idlePosition) <= 0.5f)
            {
                enemyState = EnemyStates.Idle;
                transform.position = idlePosition;
                FlipScale(playerTransform.position);
            }
            else
            {
                enemyState = EnemyStates.ToIdle;
            }
        }

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (enemyState == EnemyStates.Attacking && !attacking)
        {
            //Attack();
        }
        else if (enemyState == EnemyStates.Seeking)
        {
            Move();
            FlipScale(playerTransform.position);
        }
        else if (enemyState == EnemyStates.ToIdle)
        {
            BackToIdlePosition();
            FlipScale(idlePosition);
        }
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

    private void BackToIdlePosition()
    {
        Vector2 direction = ((Vector2)transform.position - idlePosition).normalized;

        rb.velocity = new Vector2(-direction.x * moveSpeed, rb.velocity.y);
    }

    private void Move()
    {
        Vector2 direction = (transform.position - playerTransform.position).normalized;

        rb.velocity = new Vector2(-direction.x * moveSpeed, rb.velocity.y);
    }

    // Function called in an animation event
    private void StartEnemyAttack()
    {
        AudioManager.Instance.PlaySFXSound("spiderAttack");
        attacking = true;
    }

    // Function called in an animation event
    private void EndEnemyAttack()
    {
        StartCoroutine(SlowSpeed());
        attacking = false;
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    private void UpdateAnimation()
    {
        Animations newAnimation = Animations.Idle;

        if (enemyState == EnemyStates.Attacking)
        {
            newAnimation = Animations.Attack;
        }
        else if (enemyState == EnemyStates.Seeking)
        {
            newAnimation = Animations.Run;
        }
        else if (enemyState == EnemyStates.ToIdle)
        {
            newAnimation = Animations.Run;
        }
        else if (enemyState == EnemyStates.Idle)
        {
            newAnimation = Animations.Idle;
        }

        if (animator.GetInteger("currentAnimation") != (int)newAnimation)
        {
            animator.SetInteger("currentAnimation", (int)newAnimation);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();

            if (iDamageable != null)
            {
                //Damage enemy
                iDamageable.TakeDamage(damage, transform.position);
            }
        }
    }

    private IEnumerator SlowSpeed()
    {
        moveSpeed = initialSpeed / 2;
        yield return new WaitForSeconds(timeToRecoverSpeed);
        moveSpeed = initialSpeed;
        attacking = false;
    }
}
