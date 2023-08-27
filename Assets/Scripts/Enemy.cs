using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class EnemyStats
{
    public int hp = 999;
    public int damage = 1;
    public int experienceReward = 400;
    public float moveSpeed = 1f;

    public EnemyStats(EnemyStats stats)
    {
        this.hp = stats.hp;
        this.damage = stats.damage;
        this.experienceReward = stats.experienceReward;
        this.moveSpeed = stats.moveSpeed;
    }

    internal void ApplyProgress(float progress)
    {
        this.hp = (int)(hp * progress);
        this.damage = (int)(damage * progress);
        this.experienceReward = (int)(experienceReward * progress);
        this.moveSpeed = (int)(moveSpeed * progress);
    }
}

public class Enemy : MonoBehaviour, IDamageable
{
    Transform targetDestination;
    GameObject targetGameObject;
    Character targetCharecter;

    Rigidbody2D rgdbd2d;
    [SerializeField] private EnemyData enemyData;

    public EnemyStats stats;

    private float stunned;
    
    private void Awake()
    {
        rgdbd2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (enemyData != null)
        {
            InitSprite(enemyData.animatedPrefab);
            SetStats(enemyData.stats);
            SetTarget(GameManager.instance.transform.gameObject);
        }
    }
    
    public void SetTarget(GameObject target)
    {
        targetGameObject = target;
        targetDestination = target.transform;
    }

    private void FixedUpdate()
    {
        if (stunned >= 0)
        {
            rgdbd2d.velocity = Vector2.zero;
            stunned -= Time.fixedDeltaTime;
            return;
        }
        
        Vector3 direction = (targetDestination.position - transform.position).normalized;
        rgdbd2d.velocity = direction * stats.moveSpeed;
    }

    internal void UpdateStatsForProgress(float progress)
    {
        stats.ApplyProgress(progress);
    }

    internal void SetStats(EnemyStats stats)
    {
        this.stats = new EnemyStats(stats);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == targetGameObject)
        {
            Attack();
        }
    }

    private void Attack()
    {
        if(targetCharecter == null)
        {
            targetCharecter = targetGameObject.GetComponent<Character>();
        }

        targetCharecter.TakeDamage(stats.damage);
    }

    public void TakeDamage(int damage)
    {
        stats.hp -= damage;

        if(stats.hp <= 0)
        {
            targetGameObject.GetComponent<Level>().AddExperience(stats.experienceReward);
            GetComponent<DropOnDestroy>().CheckDrop();
            Destroy(gameObject);
        }
    }

    public void Stun(float stun)
    {
        stunned = stun;
    }

    public void InitSprite(GameObject animatedPrefab)
    {
        GameObject spriteObject = Instantiate(animatedPrefab);
        spriteObject.transform.parent = transform;
        spriteObject.transform.localPosition = Vector3.zero;
    }
}
