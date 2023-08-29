using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeGoblinAttackArea : MonoBehaviour
{
    [SerializeField] private AxeGoblin axeGoblin;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();

            if (iDamageable != null)
            {
                iDamageable.TakeDamage(axeGoblin.attackDamage, transform.position);
            }
        }

    }
}
