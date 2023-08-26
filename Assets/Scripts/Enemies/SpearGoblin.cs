using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearGoblin: MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float collisionDamage = 10f;

    [Header("Behavior")]
    [SerializeField] private float attackDistance = 15f;

    [Header("Attack")]
    [SerializeField] private GameObject spear;
    [SerializeField] private Transform spearSpawnPoint;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float timeToAttack = 2f;

    private bool canAttack = true;
    private Transform playerTransform;

    private WaypointsFollower waypointsFollower;
    private Animator animator;
    private BulletBehavior bulletBehavior;

    private enum Animations
    {
        Idle,
        Run,
        Attack
    };

    private enum EnemyStates
    {
        Patroling,
        Attacking
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        waypointsFollower = GetComponent<WaypointsFollower>();
        bulletBehavior = spear.GetComponent<BulletBehavior>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Start()
    {
        animator.SetInteger("currentAnimation", (int)Animations.Run);
    }

    // Update is called once per frame
    void Update()
    {
        if (!canAttack)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < attackDistance && canAttack)
        {
            StartCoroutine(AttackCooldown());
            waypointsFollower.canMove = false;
            waypointsFollower.FlipScale(playerTransform.position);
            animator.SetInteger("currentAnimation", (int)Animations.Attack);
        }
    }

    private void Attack()
    {
        float angle = GetShotAngle(transform.position, playerTransform.position);

        Instantiate(spear, spearSpawnPoint.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
        waypointsFollower.canMove = true;

        animator.SetInteger("currentAnimation", (int)Animations.Run);
    }

    private float GetShotAngle(Vector3 startLocation, Vector3 endLocation)
    {
        float maxHeight = bulletBehavior.MaxHeight;
        float gravity = bulletBehavior.BulletGravity;
        float velocity = bulletBehavior.PhysicsBulletSpeed;

        float offsetHeight = endLocation.y - startLocation.y;
        float verticalSpeed = Mathf.Sqrt(2 * gravity * maxHeight);
        float travelTime = Mathf.Sqrt(2 * (maxHeight - offsetHeight) / gravity) + Mathf.Sqrt(2 * maxHeight / gravity);
        float horizontalSpeed = (startLocation.x - endLocation.x) / travelTime;

        return (-Mathf.Atan2(verticalSpeed / velocity, horizontalSpeed / velocity) + Mathf.PI) * Mathf.Rad2Deg;
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

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(timeToAttack);
        canAttack = true;
    }
}
