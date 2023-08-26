using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeKnockBack : MonoBehaviour
{
    [SerializeField] private Vector2 knockBackForce;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void KnockBack(Vector2 damageDirection)
    {
        Vector2 directionForce = ((Vector2)transform.position - damageDirection).normalized;

        // If the player hits the top of the object
        if (directionForce.x >= -0.5f && directionForce.x <= 0.5f)
        {
            float constant = directionForce.x / directionForce.x;

            if (constant > 0)
            {
                directionForce.x -= constant;
            }
            else
            {
                directionForce.x += constant;
            }
        }

        rb.velocity = new Vector2(knockBackForce.x * directionForce.x, knockBackForce.y);
    }
}
