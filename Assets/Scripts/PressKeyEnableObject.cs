using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PressKeyEnableObject: MonoBehaviour
{
    [SerializeField] GameObject itemToShow;
    [SerializeField] PlayerController player;
    private bool inRange;
    private bool opened;

    private void Start()
    {
        itemToShow.SetActive(false);
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
        player.canMove = false;
        opened = true;

        //Stop the player velocity 
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.zero;
    }

    public void Close()
    {
        itemToShow.SetActive(false);
        player.canMove = true;
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
