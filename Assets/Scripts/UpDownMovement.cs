using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownMovement : MonoBehaviour
{
    [SerializeField] private float distance = 4f;
    [SerializeField] private float velocity = 1f;
    [SerializeField] private GameObject objectToMove;
    [SerializeField] private bool actionToMove;

    private float topLimit;
    private float bottomLimit;
    private float t = 0.0f;
    private void Start()
    {
        topLimit = objectToMove.transform.position.y;
        bottomLimit = topLimit - distance;
    }
    
    void Update()
    {
        if (actionToMove)
        {
            return;
        }

        moveObject();

        t += velocity * Time.deltaTime;

        if (t > 1.0f)
        {
            float temp = topLimit;
            topLimit = bottomLimit;
            bottomLimit = temp;
            t = 0.0f;
        }
    }

    public void moveObject()
    {
        Vector3 currentPosition = objectToMove.transform.position;
        objectToMove.transform.position = new Vector3(currentPosition.x, Mathf.Lerp(topLimit, bottomLimit, t), currentPosition.z);
    }
}
