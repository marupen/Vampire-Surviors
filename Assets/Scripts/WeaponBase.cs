using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionsOfAttack
{
    None,
    Forward,
    LeftRight,
    UpDown
}

public abstract class WeaponBase : MonoBehaviour
{
    PlayerMove playerMove;

    public WeaponData weaponData;

    public WeaponStats weaponStats;

    float timer;

    Character wielder;
    public Vector2 vectorOfAttack;
    [SerializeField] DirectionsOfAttack attackDirection;

    private void Awake()
    {
        playerMove = GetComponentInParent<PlayerMove>();
    }

    public void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            Attack();
            timer = weaponStats.timeToAttack;
        }
    }

    public void ApplyDamage(Collider2D[] colliders)
    {
        int damage = GetDamage();
        for (int i = 0; i < colliders.Length; i++)
        {
            IDamageable e = colliders[i].GetComponent<IDamageable>();
            if (e != null)
            {
                ApplyDamage(colliders[i].transform.position, damage, e);
            }
        }
    }

    public void ApplyDamage(Vector3 position, int damage, IDamageable e)
    {
        PostDamage(damage, position);
        e.TakeDamage(damage);
        ApplyAdditionalEffects(e);
    }

    private void ApplyAdditionalEffects(IDamageable damageable)
    {
        damageable.Stun(weaponStats.stun);
    }

    public virtual void SetData(WeaponData wd)
    {
        weaponData = wd;

        weaponStats = new WeaponStats(wd.stats);
    }

    public abstract void Attack();

    public int GetDamage()
    {
        int damage = (int)(weaponData.stats.damage * wielder.damageBonus);
        return damage;
    }

    public virtual void PostDamage(int damage, Vector3 targetPosition)
    {
        MessageSystem.instance.PostMessage(damage.ToString(), targetPosition);
    }

    public void Upgrade(UpgradeData upgradeData)
    {
        weaponStats.Sum(upgradeData.weaponUpgradeStats);
    }

    public void AddOwner(Character character)
    {
        wielder = character;
    }

    public void UpdateVectorOfAttack()
    {
        if(attackDirection == DirectionsOfAttack.None)
        {
            vectorOfAttack = Vector2.zero;
            return;
        }

        switch (attackDirection)
        {
            case DirectionsOfAttack.Forward:
                vectorOfAttack.x = playerMove.lastHorizontalCoupledVector;
                vectorOfAttack.y = playerMove.lastVerticalCoupledVector;
                break;
            case DirectionsOfAttack.LeftRight:
                vectorOfAttack.x = playerMove.lastHorizontalDeCoupledVector;
                vectorOfAttack.y = 0f;
                break;
            case DirectionsOfAttack.UpDown:
                vectorOfAttack.x = 0f;
                vectorOfAttack.y = playerMove.lastVerticalDeCoupledVector;
                break;
        }
        vectorOfAttack = vectorOfAttack.normalized;
    }

    public GameObject SpawnProjectile(GameObject projectilePrefab, Vector3 position)
    {
        GameObject projectileGO = Instantiate(projectilePrefab);
        projectileGO.transform.position = position;
        
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.SetDirection(vectorOfAttack.x, vectorOfAttack.y);
        projectile.SetStats(this);

        return projectileGO;
    }
}
