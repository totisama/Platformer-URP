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

    // Update is called once per frame
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

        Vector3 target = waypoints[currentWaypointIndex].position;

        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        FlipScale(target);
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
