using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints;
    [SerializeField] private int currencyWorth;
    [SerializeField] private string deathSoundName;

    private bool isDestroyed = false;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dmg"></param>
    public void TakeDamage(int dmg) // This allows enemies to take damage
    {
        
        hitPoints -= dmg;

        if (hitPoints <= 0 && !isDestroyed) 
        {
            // Tells EnemySpawner that an enemy is destroyed
            EnemySpawner.onEnemyDestroy.Invoke();

            // Increase the currency the player has by the currencyWorth of the enemy
            LevelManager.main.IncreaseCurrency(currencyWorth); 

            FindObjectOfType<AudioManager>().Play(deathSoundName);

            isDestroyed = true;
            Destroy(gameObject); 
        }
    }

    public void HealthDamage() // This allows the player to take damage
    {
        LevelManager.healthPoints -= hitPoints;
    }
}
