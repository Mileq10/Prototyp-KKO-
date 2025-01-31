using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatResolver : MonoBehaviour
{
    public EnemyData CurrentEnemyData => currentEnemyData;
    [SerializeField] private EnemyData[] enemyData;
    private EnemyData currentEnemyData;
    private GameObject currentEnemyModel;
    private RuntimeEnemy currentEnemy;
    private List<EnemyData> upcomingEnemies;
    public RuntimeEnemy CurrentEnemy => currentEnemy;

    private void Start()
    {
        upcomingEnemies = new List<EnemyData>();
        upcomingEnemies.AddRange(enemyData);
    }
    public void ResolveCombat(int damage)
    {
        if (currentEnemy == null)
        {
            return;
        }
        if(damage < 0)
        {
            return;
        }
        currentEnemy.Damage(damage+1);
    }
    private void SpawnEnemy(EnemyData enemyData)
    {
        if (currentEnemy != null)
        {
            ClearCurrentEnemy();
        }
        currentEnemy = new RuntimeEnemy(enemyData);
        currentEnemyData = currentEnemy.EnemyData;
        SpawnEnemyModel(currentEnemy.EnemyData.Prefab);
        currentEnemy.HealthModel.OnDeath += OnEnemyDeath;
    }

    private void SpawnEnemyModel(GameObject prefab)
    {
        if(prefab == null)
        {
            return;
        }
        currentEnemyModel = Instantiate(prefab, transform);
        currentEnemyModel.transform.localPosition = Vector3.zero;
        currentEnemyModel.transform.localRotation = Quaternion.identity;
    }

    private void ClearCurrentEnemy()
    {
        currentEnemy.HealthModel.OnDeath -= OnEnemyDeath;
        currentEnemy.OnDestroy();
        if (currentEnemyModel != null)
            Destroy(currentEnemyModel);
        currentEnemyModel = null;
        currentEnemy = null;
    }

    private void OnEnemyDeath()
    {
        if (upcomingEnemies.Count == 0) 
        {
            ClearCurrentEnemy();
            return;
        }
        var nextEnemy = upcomingEnemies[0];
        upcomingEnemies.RemoveAt(0);
        SpawnEnemy(nextEnemy);
    }

    internal void Init()
    {
        OnEnemyDeath();
    }
}
