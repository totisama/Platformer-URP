using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMage : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float collideDamage = 10f;
    [SerializeField] internal float closeAttackDamage = 10f;
    [SerializeField] private int fireballDamage = 15;

    [Header("Behavior")]
    [SerializeField] private GameObject[] teleportPoints;
    [SerializeField] private float timeToTeleport = 7f;
    [SerializeField] private float distanceToAwake = 15f;
    [SerializeField] private float timeToMove = 1f;

    [Header("Attack")]
    [SerializeField] private GameObject fireball;
    [SerializeField] private Transform fireballSpawnPoint;
    [SerializeField] private float fireballAttackCooldown = 5f;
    [SerializeField] private float distanceToShortAttack = 3f;
   
    [Header("Canvas")]
    [SerializeField] private RectTransform rectTransform;

    private Transform playerTransform;
    private float nextFireballAttack;
    private float nextTeleport;
    private int teleportPoint;
    private bool teleporting;
    private bool ableToMove = true;
    private EnemyStates enemyState;

    private Animator animator;
    private Rigidbody2D rb;

    private enum Animations
    {
        Idle,
        Run,
        Close_Attack,
        Far_Attack,
        Teleport,
        Appear
    }

    private enum EnemyStates
    {
        Idle,
        Chasing,
        Attacking,
        Teleporting
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Start()
    {
        nextTeleport = timeToTeleport;
        nextFireballAttack = fireballAttackCooldown;
    }

    void Update()
    {
        if (teleporting || enemyState == EnemyStates.Attacking || !ableToMove)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > distanceToAwake)
        {
            animator.SetInteger("currentAnimation", (int)Animations.Idle);
            nextTeleport = Time.time + timeToTeleport;
            nextFireballAttack = Time.time + fireballAttackCooldown;
            if (rb.velocity.x > 0f)
            {
                ChosePointToTeleport();
            }

            return;
        }

        if (distanceToPlayer <= distanceToShortAttack)
        {
            StartCloseAttack();
            return;
        }

        if (Time.time > nextTeleport)
        {
            ChosePointToTeleport();
            return;
        }
        else if (Time.time > nextFireballAttack)
        {
            StartFarAttack();
            return;
        }

        if (Mathf.Abs(transform.position.y - playerTransform.position.y) < 2.0f)
        {
            ChangeCurrentState(EnemyStates.Chasing);
        }
        else
        {
            animator.SetInteger("currentAnimation", (int)Animations.Idle);
        }
    }

    private void FixedUpdate()
    {
        if (enemyState == EnemyStates.Chasing)
        {
            ChasePlayer();
            FlipScale(playerTransform.position);
        }
    }

    private void StartFarAttack()
    {
        ChangeCurrentState(EnemyStates.Attacking);
        rb.velocity = Vector2.zero;
        animator.SetInteger("currentAnimation", (int)Animations.Far_Attack);
    }
    
    private void StartCloseAttack()
    {
        ChangeCurrentState(EnemyStates.Attacking);
        rb.velocity = Vector2.zero;
        animator.SetInteger("currentAnimation", (int)Animations.Close_Attack);
    }

    private void CloseAttack()
    {
        ChangeCurrentState(EnemyStates.Idle);
        StartCoroutine(EnableMovement());
    }

    private void FarAttack()
    {
        nextFireballAttack = Time.time + fireballAttackCooldown;
        ChangeCurrentState(EnemyStates.Idle);
        ChosePointToTeleport();
        StartCoroutine(EnableMovement());
    }

    private void ThrowFireball()
    {
        Vector3 target = playerTransform.position;

        FlipScale(target);  
        float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;

        GameObject fireballGO = Instantiate(fireball, fireballSpawnPoint.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));

        fireballGO.GetComponent<BulletBehavior>().SetDamage(fireballDamage);
    }

    private void ChasePlayer()
    {
        animator.SetInteger("currentAnimation", (int)Animations.Run);

        Vector2 direction = (transform.position - playerTransform.position).normalized;

        rb.velocity = new Vector2(-direction.x * moveSpeed, rb.velocity.y);
    }

    private void ChosePointToTeleport()
    {
        teleportPoint = Random.Range(0, teleportPoints.Length);
        rb.velocity = Vector2.zero;
        teleporting = true;

        animator.SetInteger("currentAnimation", (int)Animations.Teleport);
    }

    private void Teleport()
    {
        animator.SetInteger("currentAnimation", (int)Animations.Appear);

        transform.position = teleportPoints[teleportPoint].GetComponent<Transform>().position;
    }
    
    private void Appear()
    {
        nextTeleport = Time.time + timeToTeleport;
        animator.SetInteger("currentAnimation", (int)Animations.Idle);
        teleporting = false;
    }

    private void ChangeCurrentState(EnemyStates newState)
    {
        enemyState = newState;
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

    private IEnumerator EnableMovement()
    {
        ableToMove = false;
        yield return new WaitForSeconds(timeToMove);
        ableToMove = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();

            if (iDamageable != null)
            {
                //Damage enemy
                iDamageable.TakeDamage(collideDamage, transform.position);
            }
        }
    }
}
