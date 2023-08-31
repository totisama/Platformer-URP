using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxEffect;

    private float startPosition;

    private void Start()
    {
        startPosition = transform.position.x;
    }

    private void Update()
    {
        float distance = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3 (startPosition + distance, transform.position.y, transform.position.z);
    }
}
