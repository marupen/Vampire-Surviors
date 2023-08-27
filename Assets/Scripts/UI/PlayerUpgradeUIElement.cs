using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUpgradeUIElement : MonoBehaviour
{
    [SerializeField] PlayerPersistentUpgrades upgrade;

    [SerializeField] TextMeshProUGUI upgradeName;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] TextMeshProUGUI price;

    [SerializeField] DataConteiner dataConteiner;

    private void Start()
    {
        UpdateElement();
    }

    public void Upgrade()
    {
        PlayerUpgrades playerUpgrade = dataConteiner.upgrades[(int)upgrade];

        if (playerUpgrade.level >= playerUpgrade.max_level) return;
        if(dataConteiner.coins >= playerUpgrade.costToUpgrade)
        {
            dataConteiner.coins -= playerUpgrade.costToUpgrade;
            playerUpgrade.level += 1;
            UpdateElement();
        }
    }

    void UpdateElement()
    {
        PlayerUpgrades playerUpgrade = dataConteiner.upgrades[(int)upgrade];

        upgradeName.text = upgrade.ToString();
        level.text = playerUpgrade.level.ToString();
        price.text = playerUpgrade.costToUpgrade.ToString();
    }
}
