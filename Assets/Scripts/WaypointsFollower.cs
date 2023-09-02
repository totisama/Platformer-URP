using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsFollower : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed;
    public bool canMove = true;

    [Header("Canvas")]
    [SerializeField] private RectTransform rectTransform;

    private int currentWaypointIndex = 0;
    private Vector3 currentTarget;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(!canMove)
        {
            return;
        }

        if (Vector2.Distance(waypoints[currentWaypointIndex].position, transform.position) <= 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length) 
            {
                currentWaypointIndex = 0;
            }
        }

        currentTarget = waypoints[currentWaypointIndex].position;

        FlipScale(currentTarget);
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }

        Move();
    }

    private void Move()
    {
        Vector2 direction = (transform.position - currentTarget).normalized;

        rb.velocity = new Vector2(-direction.x * moveSpeed, rb.velocity.y);
    }

    internal void CanMove(bool ableToMove)
    {
        canMove = ableToMove;

        if (!ableToMove)
        {
            rb.velocity = Vector3.zero;
        }

    }

    public void FlipScale(Vector3 target)
    {
        Vector3 scale = transform.localScale;

        if (target.x < transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x) * -1;
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
        }

        transform.localScale = scale;

        if (rectTransform)
        {
            rectTransform.localScale = scale;
        }
    }
}
