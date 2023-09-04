using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarAttack : MonoBehaviour
{

    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private int collisionDamage;
    [Header("Shooting")]
    [SerializeField] private float fireRate;
    [SerializeField] private float distanceToShoot;

    private float nextFire;

    Animator anim;
    Transform playerTransform;


    void Start()
    {
        anim = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= distanceToShoot && Time.time > nextFire)
        {
            SetShootAnimation(true);
        }
        else
        {
            SetShootAnimation(false);
        }
    }

    private void Shoot()
    {
        Instantiate(arrow, arrowSpawnPoint.position, Quaternion.Euler(new Vector3(0f, 0f, -180.0f)));
    }

    private void FinishShoot()
    {
        nextFire = Time.time + fireRate;
        SetShootAnimation(false);
    }

    private void SetShootAnimation(bool shoot)
    {
        anim.SetBool("shoot", shoot);
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
