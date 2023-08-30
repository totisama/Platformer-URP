using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PressKeyEnableObject: MonoBehaviour
{
    [SerializeField] GameObject itemToShow;
    
    private PlayerController player;
    private bool inRange;
    private bool opened;

    private void Start()
    {
        itemToShow.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
        player.LockPlayer();
        opened = true;
    }

    public void Close()
    {
        itemToShow.SetActive(false);
        player.UnlockPlayer();
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
