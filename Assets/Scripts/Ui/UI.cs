using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{   
    public int deaths = 0;
    public int coins = 0;

    public TextMeshProUGUI death_text;
    public TextMeshProUGUI coin_text;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventBus.Singleton.PlayerDeath += () => coins += 1;
        EventBus.Singleton.CoinCollect += () => deaths += 1;
    }

    // Update is called once per frame
    void Update()
    {
        death_text.text = "Total Deaths: " + deaths;
        coin_text.text = "Coins Collected: " + coins;
    }
}
