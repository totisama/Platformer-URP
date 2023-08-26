using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMageCloseAttack : MonoBehaviour
{
    [SerializeField] private SkeletonMage skeletonMage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();

            if (iDamageable != null)
            {
                iDamageable.TakeDamage(skeletonMage.closeAttackDamage, transform.position);
            }
        }
    }
}
