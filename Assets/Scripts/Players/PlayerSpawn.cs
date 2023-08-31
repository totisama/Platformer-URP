using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    internal Vector3 spawnPosition;

    private void Start()
    {
        spawnPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnPoint"))
        {
            Vector3 spawnPos = collision.transform.position;

            if (spawnPosition.x < spawnPos.x)
            {
                spawnPosition = spawnPos;
            }
        }
    }
}
