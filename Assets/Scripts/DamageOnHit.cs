using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private Vector2 knockBackMultiplier = new Vector2(1.0f, 1.0f);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();

            if (iDamageable != null)
            {
                iDamageable.TakeDamage(damage, transform.position, knockBackMultiplier);
            }
        }
    }
}
