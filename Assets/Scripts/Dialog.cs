using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private string[] dialogs;
    [Header("Typing")]
    [SerializeField] private float typingSpeed;
    [SerializeField] private float nextDialog;

    private int currentDialog;
    private bool opened;
    private PlayerController player;

    private void Start()
    {
        dialogPanel.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OpenDialog()
    {
        opened = true;
        player.LockPlayer();
        dialogPanel.SetActive(true);
        StartCoroutine(Typing());
    }

    private void ResetDialog()
    {
        dialogText.text = "";
        currentDialog = 0;
        dialogPanel.SetActive(false);
        player.UnlockPlayer();
        GameManager.Instance.ChangePlayerPrefab();
    }

    private void NextDialog()
    {
        if (currentDialog < dialogs.Length - 1)
        {
            currentDialog += 1;
            dialogText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            ResetDialog();
        }
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogs[currentDialog].ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(nextDialog);
        NextDialog();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !opened)
        {
            OpenDialog();
        }
    }
}
