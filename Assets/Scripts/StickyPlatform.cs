using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    [SerializeField] private float speed;

    internal Vector2 direction;

    Rigidbody2D playerRB;
    PlayerController playerController;
    ElevatorBehavior elevatorBehavior;

    private void Start()
    {
        elevatorBehavior = GetComponentInParent<ElevatorBehavior>();
    }

    private void FixedUpdate()
    {
        if (!playerRB)
        {
            return;
        }

        playerRB.velocity = new Vector2(direction.x * speed, playerRB.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerRB = collision.GetComponent<Rigidbody2D>();
            playerController = collision.GetComponent<PlayerController>();
            playerController.onMovingPlatform = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerRB = null;
            playerController.onMovingPlatform = false;
            playerController = null;
        }
    }
}
