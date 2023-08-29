using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackArea : MonoBehaviour
{
    [SerializeField] private MeleeAttack meleeAttack;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();

            if (iDamageable != null)
            {
                iDamageable.TakeDamage(meleeAttack.attackDamage, transform.position);
            }
        }

    }
}
