using JetBrains.Annotations;
using System;
using UnityEngine;

public class RuntimeEnemy
{
    public EnemyData EnemyData {  get; private set; }
    public HealthModel HealthModel { get; private set; }
    public RuntimeEnemy(EnemyData enemyData)
    {
        EnemyData = enemyData;
        HealthModel = new(enemyData.Health);

    }
    public void Damage(int damage)
    {
        HealthModel.Damage(damage);
    }

    internal void OnDestroy()
    {
        
    }
}
