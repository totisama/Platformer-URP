using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ElevatorBehavior : MonoBehaviour
{
    [SerializeField] internal Modes mode;
    [SerializeField] private GameObject toRotateMove;
    [SerializeField] private GameObject toMove;
    [SerializeField] private float distance;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float moveSpeed;

    private float initial;
    private float limit;
    private float t = 0.0f;
    private bool clockwise = true;
    StickyPlatform stickyPlatform;

    internal enum Modes
    {
        Horizontal,
        Vertical
    }

    private void Awake()
    {
        stickyPlatform = GetComponentInChildren<StickyPlatform>();
    }

    private void Start()
    {
        if (mode == Modes.Horizontal)
        {
            initial = toMove.transform.position.x;
        }
        else if (mode == Modes.Vertical)
        {
            initial = toMove.transform.position.y;
        }

        limit = initial + distance;
        
    }

    private void Update()
    {
        MoveObject(toMove);
        MoveRotateObjects();

        t += moveSpeed * Time.deltaTime;

        if (t > 1.0f)
        {
            ChangeDirection();
            float temp = limit;

            limit = initial;
            initial = temp;
            t = 0.0f;
            clockwise = !clockwise;
        }
    }

    private void ChangeDirection()
    {
        if (stickyPlatform)
        {
            stickyPlatform.direction = limit > initial ? new Vector2(-1, 0) : new Vector2(1, 0);
        }
    }

    private void MoveObject(GameObject objectToMove)
    {
        Vector3 currentPosition = objectToMove.transform.position;

        float lerp = Mathf.Lerp(initial, limit, t);
        Vector2 newPosition = Vector2.zero;

        if (mode == Modes.Horizontal)
        {
            newPosition = new Vector2(lerp, currentPosition.y);
        }
        else if (mode == Modes.Vertical)
        {
            newPosition = new Vector2(currentPosition.x, lerp);
        }

        objectToMove.transform.position = newPosition;
    }
    
    private void MoveRotateObjects()
    {
        MoveObject(toRotateMove);

        Vector3 rotateVector = clockwise ? new Vector3(0, 0, -1) : new Vector3(0, 0, 1);

        toRotateMove.transform.Rotate(rotateVector * rotateSpeed * Time.deltaTime);
    }
}
