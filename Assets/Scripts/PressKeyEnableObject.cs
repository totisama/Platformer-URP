using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PressKeyEnableObject: MonoBehaviour
{
    [SerializeField] GameObject itemToShow;
    
    private PlayerController playerController;
    private PlayerAttack playerAttack;
    private bool inRange;
    private bool opened;

    private void Start()
    {
        itemToShow.SetActive(false);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerAttack = player.GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.E) && inRange)
        {
            if (!opened)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
    }

    private void Open()
    {
        itemToShow.SetActive(true);
        playerController.LockPlayer();
        playerAttack.canAttack = false;
        opened = true;
    }

    public void Close()
    {
        itemToShow.SetActive(false);
        playerController.UnlockPlayer();
        playerAttack.canAttack = true;
        opened = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
