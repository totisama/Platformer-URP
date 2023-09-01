using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TMP_Text coinsAmount;
    [SerializeField] private TMP_Text inventoryCoinsAmount;
    [SerializeField] private CameraController cameraController;

    [Header("Player Spawn")]
    [SerializeField] private GameObject bowPlayer;
    [SerializeField] private GameObject extraHealth;
    [SerializeField] private Slider extraHealthSlider;
    [SerializeField] private Slider healthSlider;

    private GameObject currentPlayer;
 
    private int coins = 100;

    public int Coins { get { return coins; } private set { coins = value; } }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        coinsAmount.SetText(Coins.ToString());
        inventoryCoinsAmount.SetText(Coins.ToString());
        currentPlayer = GameObject.FindGameObjectWithTag("UnarmedPlayer");
    }

    public void ChangePlayerPrefab()
    {
        bowPlayer.GetComponent<SpriteRenderer>().enabled = true;
        bowPlayer.GetComponent<PlayerController>().UnlockPlayer();
        bowPlayer.GetComponent<PlayerAttack>().canAttack = true;

        bowPlayer.transform.position = currentPlayer.transform.position;
        bowPlayer.SetActive(true);
        cameraController.ChangePlayer(bowPlayer.transform);

        Destroy(currentPlayer);
        currentPlayer = bowPlayer;
    }

    public void RespawnPlayer(Vector3 position)
    {
        currentPlayer.transform.position = position;
    }

    public void IncreaseCoins(int amount)
    {
        Coins += amount;

        coinsAmount.SetText(Coins.ToString());
        inventoryCoinsAmount.SetText(Coins.ToString());
    }
    
    public void DecreaseCoins(int amount)
    {
        Coins -= amount;

        coinsAmount.SetText(Coins.ToString());
        inventoryCoinsAmount.SetText(Coins.ToString());
    }
}
