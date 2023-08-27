using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int maxHP = 1000;
    public int currentHP = 1000;

    public int armor = 0;

    public float hpRegenerationRate = 1f;
    public float hpRegenerationTimer;

    public float damageBonus;

    [SerializeField] StatusBar hpBar;

    [HideInInspector] public Level level;
    [HideInInspector] public Coins coins;
    private bool isDead;

    [SerializeField] DataConteiner dataConteiner;

    private void Awake()
    {
        level = GetComponent<Level>();
        coins = GetComponent<Coins>();
    }

    private void Start()
    {
        ApplyPersistantUpgrades();

        hpBar.SetState(currentHP, maxHP);
    }

    private void ApplyPersistantUpgrades()
    {
        int hpUpgradeLevel = dataConteiner.GetUpgradeLevel(PlayerPersistentUpgrades.HP);

        maxHP += maxHP / 10 * hpUpgradeLevel;

        int damageUpgradeLevel = dataConteiner.GetUpgradeLevel(PlayerPersistentUpgrades.Damage);

        damageBonus = 1f + 0.1f * damageUpgradeLevel;
    }

    private void Update()
    {
        hpRegenerationTimer += Time.deltaTime * hpRegenerationRate;

        if(hpRegenerationTimer > 1f)
        {
            Heal(1);
            hpRegenerationTimer -= 1f;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        ApplyArmor(ref damage);

        currentHP -= damage;

        if(currentHP <= 0)
        {
            GetComponent<CharacterGameOver>().GameOver();
            isDead = true;
        }
        hpBar.SetState(currentHP, maxHP);
    }

    private void ApplyArmor(ref int damage)
    {
        damage -= armor;
        if (damage < 0) damage = 0;
    }

    public void Heal(int amount)
    {
        if(currentHP <= 0)
        {
            return;
        }

        currentHP += amount;
        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        hpBar.SetState(currentHP, maxHP);
    }
}
