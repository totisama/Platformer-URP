using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] GameObject finishPanel;

    private PlayerController playerController;

    private void Start()
    {
        finishPanel.SetActive(false);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        finishPanel.SetActive(true);
        playerController.LockPlayer();
    }
}
