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
    [SerializeField] private GameObject bowPlayerPrefab;
    [SerializeField] private GameObject extraHealth;
    [SerializeField] private Slider extraHealthSlider;
    [SerializeField] private Slider healthSlider;

    private GameObject currentPlayer;
 
    private int coins;

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
        currentPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    public void ChangePlayerPrefab()
    {
        Vector3 currentPosition = currentPlayer.transform.position;
        GameObject bowPlayer = Instantiate(bowPlayerPrefab, currentPosition, Quaternion.identity);
        cameraController.ChangePlayer(bowPlayer.transform);

        PlayerHealth playerHealth = bowPlayer.GetComponent<PlayerHealth>();
        playerHealth.SetHealthObjects(extraHealth, healthSlider, extraHealthSlider);

        Destroy(currentPlayer);
        currentPlayer = bowPlayer;
    }

    public void RespawnPlayer(Vector3 position)
    {
        GameObject bowPlayer = Instantiate(bowPlayerPrefab, position, Quaternion.identity);
        cameraController.ChangePlayer(bowPlayer.transform);

        PlayerHealth playerHealth = bowPlayer.GetComponent<PlayerHealth>();
        playerHealth.SetHealthObjects(extraHealth, healthSlider, extraHealthSlider);

        Destroy(currentPlayer);
        currentPlayer = bowPlayer;
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
