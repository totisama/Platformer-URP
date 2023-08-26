using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour, IDamageable
{
    [SerializeField] private float hitsToDestroy = 3f;
    [Header("On Drop")]
    [SerializeField] private bool dropObjectsOnDestroy;
    [SerializeField] private GameObject dropObject;
    [SerializeField] private bool physicsDrop;
    [Header("Random")]
    [SerializeField] private bool randomAmount;
    [Tooltip("Not included")]
    [SerializeField] private int maxAmount = 0;
    [Header("Controlled")]
    [SerializeField] private int amount = 0;

    private Animator animator;
    private Collider2D col;

    void Start()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    public void TakeDamage(float damage, Vector2 damageDirection)
    {
        hitsToDestroy -= 1f;

        if (hitsToDestroy <= 0)
        {
            animator.SetTrigger("destroy");
            col.enabled = false;

            if (!dropObjectsOnDestroy)
            {
                return;
            }

            SpawnObjects(randomAmount);
        }
    }

    private void SpawnObjects(bool random)
    {
        int objectsAmount = amount;

        if (random) {
            objectsAmount = Random.Range(0, maxAmount);
        }

        if (physicsDrop)
        {
            for (int i = 0; i < objectsAmount; i++)
            {
                float xImpulse = Random.Range(-3f, 4.0f);
                float yImpulse = Random.Range(0f, 6.0f);

                GameObject instance = Instantiate(dropObject, transform.position, Quaternion.identity);
                Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();

                rb.gravityScale = 1f;
                rb.AddForce(new Vector2(xImpulse, yImpulse), ForceMode2D.Impulse);
            }
        }
        else
        {
            GameObject instance = Instantiate(dropObject, transform.position, Quaternion.identity);
            SpiderEnemy spider = instance.GetComponent<SpiderEnemy>();
        }

    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
