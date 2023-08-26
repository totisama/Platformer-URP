using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectable : MonoBehaviour
{
    [SerializeField] private CollectableType type;
    [SerializeField] private int value = 1;

    private Animator animator;
    private Rigidbody2D rb;

    private enum CollectableType
    {
        coin
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void CollectItem()
    {
        if (type == CollectableType.coin)
        {
            GameManager.Instance.IncreaseCoins(value);
        }

        animator.SetBool("collected", true);
    }

    private void DestroyItem()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollectItem();
        }
        else if (collision.gameObject.CompareTag("Terrain"))
        {
            rb.bodyType = RigidbodyType2D.Static;
            Destroy(gameObject, 3f);
        }
    }
}
