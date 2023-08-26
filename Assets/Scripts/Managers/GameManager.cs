using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] TMP_Text coinsAmount;
    [SerializeField] TMP_Text inventoryCoinsAmount;
 
    private int coins = 50;

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
