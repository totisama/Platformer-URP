using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private int extraDamageValue;
    public float fireRate = 1f;
    private float initialfireRate = 1f;

    private PlayerController controller;
    private float nextFire;
    private int extraDamage = 0;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        initialfireRate = fireRate;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextFire && !controller.isClimbing)
        {
            nextFire = Time.time + fireRate;
            controller.isAttacking = true;
        }
    }

    public void Attack()
    {
        AudioManager.Instance.PlaySFXSound("arrowShot");
        GameObject arrowGO = Instantiate(arrow, arrowSpawnPoint.position, transform.rotation);
        if (extraDamage > 0)
        {
            BulletBehavior bulletBehavior = arrowGO.GetComponent<BulletBehavior>();
            bulletBehavior.IncreaseDamage(extraDamage);
        }
        controller.isAttacking = false;
    }

    public void UsePotion(ActionType potionType, float timeToConsume)
    {
        if (potionType == ActionType.attackDamage)
        {
            extraDamage = extraDamageValue;
            StartCoroutine(DecreaseDamage(timeToConsume));
        }
        else if (potionType == ActionType.attackSpeed)
        {
            fireRate = 0.5f;
            StartCoroutine(IncreaseAttackSpeed(timeToConsume));
        }
    }
    IEnumerator DecreaseDamage(float timeToConsume)
    {
        yield return new WaitForSeconds(timeToConsume);
        extraDamage = 0;
    }
    
    IEnumerator IncreaseAttackSpeed(float timeToConsume)
    {
        yield return new WaitForSeconds(timeToConsume);
        fireRate = initialfireRate;
    }
}
