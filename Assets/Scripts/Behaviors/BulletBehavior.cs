using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [Header("General Stats")]
    [SerializeField] private LayerMask whatDestroysBullet;
    [SerializeField] private float destroyTime = 3f;
    [SerializeField] private BulletType bulletType;
    [SerializeField] private float bulletDamage = 15f;

    [Header("Normal Stats")]
    [SerializeField] private float normalBulletSpeed = 15f;

    [Header("Physics Stats")]
    [SerializeField] private float physicsBulletSpeed = 15f;
    [SerializeField] private float bulletGravity = 3f;
    [SerializeField] private float maxHeight = 3f;

    public float PhysicsBulletSpeed { get { return physicsBulletSpeed; } private set { physicsBulletSpeed = value; } }
    public float BulletGravity { get { return bulletGravity; } private set { bulletGravity = value; } }
    public float MaxHeight { get { return maxHeight; } private set { maxHeight = value; } }

    private Rigidbody2D rb;
    private enum BulletType
    {
        normal,
        physics
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        SetRBStats();
        DestroyBullet();
    }

    private void FixedUpdate()
    {
        if (bulletType == BulletType.physics)
        {
            transform.right = rb.velocity;
        }
    }

    private void SetRBStats()
    {
        if (bulletType == BulletType.normal)
        {
            rb.velocity = transform.right * normalBulletSpeed;
            rb.gravityScale = 0f;
        }
        else if (bulletType == BulletType.physics)
        {
            rb.velocity = transform.right * PhysicsBulletSpeed;
            rb.gravityScale = BulletGravity;
        }
    }
    
    private void DestroyBullet()
    {
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Is the collision is on the specific layerMask
        if ((whatDestroysBullet.value & (1 << collision.gameObject.layer)) > 0)
        {
            IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();

            if (iDamageable != null)
            {
                //Damage enemy
                iDamageable.TakeDamage(bulletDamage, transform.position);
                AudioManager.Instance.PlaySFXSound("hit");
            }

            Destroy(gameObject);
        }
    }

    public void IncreaseDamage(int extraDamage)
    {
        bulletDamage += extraDamage;
    }
    
    public void SetDamage(int damage)
    {
        bulletDamage = damage;
    }
}
