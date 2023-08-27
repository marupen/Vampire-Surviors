using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsText : MonoBehaviour
{
    [SerializeField] DataConteiner dataConteiner;
    TMPro.TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = "Coins: " + dataConteiner.coins.ToString();
    }
}
