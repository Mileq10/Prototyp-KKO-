using System;
using UnityEngine;

public class HealthModel
{
    public int Health {  get; set; }
    public int MaxHealth {  get; private set; }
    public int MinHealth {  get; private set; }
    public bool IsAlive => Health >= MinHealth;
    public event Action OnDeath;
    public HealthModel(int health)
    {
        MaxHealth = health;
        MinHealth = 1;
        Health = health;
    }
    public void Damage(int value)
    {
        Health -= value;
        HandleDeath();
    }

    private void HandleDeath()
    {
        if(IsAlive)
        {
            return;
        }
        OnDeath?.Invoke();
    }

    internal float GetPercentage()
    {
        return (float)Health / MaxHealth;
    }
}
