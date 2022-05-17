using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health
{
    public int CurrentHealth => _currentHealth;
    private int _currentHealth;

    public Health(int startingHealth)
    {
        _currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
    }

    public void Heal(int toHeal)
    {
        _currentHealth += toHeal;
    }
}
