using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField] DataConteiner data;
    [SerializeField] TMPro.TextMeshProUGUI coinsCountText;

    public void Add(int count)
    {
        data.coins += count;
        coinsCountText.text = "COINS: " + data.coins.ToString();
    }
}
