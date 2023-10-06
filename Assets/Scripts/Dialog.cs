using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private Sprite dialogImage;
    [SerializeField] private Image imageObject;
    [SerializeField] private string[] dialogs;
    [SerializeField] private string tagToSearch;
    [SerializeField] private bool startDialog;
    [Header("Typing")]
    [SerializeField] private float typingSpeed;
    [SerializeField] private float nextDialog;
    [SerializeField] private bool skipDialog;

    private int currentDialog;
    private bool opened;
    private PlayerController player;
    private PressKeyEnableObject enableObject;

    private void Start()
    {
        dialogPanel.SetActive(false);
        player = GameObject.FindGameObjectWithTag(tagToSearch).GetComponent<PlayerController>();
        enableObject = GetComponent<PressKeyEnableObject>();
    }

    private void OpenDialog()
    {
        opened = true;
        player.LockPlayer();
        dialogPanel.SetActive(true);
        imageObject.sprite = dialogImage;
        StartCoroutine(Typing());
        if (enableObject)
        {
            enableObject.ableToOpen = false;
        }
    }

    private void ResetDialog()
    {
        dialogText.text = "";
        currentDialog = 0;
        dialogPanel.SetActive(false);
        player.UnlockPlayer();

        if (startDialog)
        {
            GameManager.Instance.ChangePlayerPrefab();        
        }

        if (enableObject)
        {
            enableObject.ableToOpen = true;
        }
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
        if (!opened && collision.CompareTag(tagToSearch))
        {
            if (skipDialog)
            {
                GameManager.Instance.ChangePlayerPrefab();
            }
            else
            {
                OpenDialog(); 
            }
        }
    }
}
