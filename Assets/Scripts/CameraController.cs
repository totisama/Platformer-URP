using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float xOffset = 2f;
    [SerializeField] float yOffset = 2f;
    [SerializeField] Transform playerTransform;

    private void Update()
    {
        Vector3 position = playerTransform.position;

        transform.position = new Vector3(position.x + xOffset, position.y + yOffset, transform.position.z);
    }
}
