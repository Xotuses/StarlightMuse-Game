using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyWorth = 50; 

    private bool isDestroyed = false;

    public void TakeDamage(int dmg) 
    {
        hitPoints -= dmg;

        if (hitPoints <= 0 && !isDestroyed) 
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth); // Increase the currency the player has by 50 when the enemy is destroyed
            isDestroyed = true;
            Destroy(gameObject); 
        }
    }

    public void HealthDamage() 
    {
        LevelManager.healthPoints -= hitPoints;
    }
}
