using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BassBlaster : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float targetingRange;
    [SerializeField] private float aps; // Bullets per Second
    [SerializeField] private float slowTime;

    [Header("Misc")]
    [SerializeField] private LayerMask enemyMask;
    
    private float timeUntilFire;

    /// <summary>
    /// This method tracks real time, when 1/aps of a second has passed, the tower slows enemies.
    /// it then resets the time until fire.
    /// </summary>
    private void Update() 
    {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / aps) 
        {
            SlowEnemies();

            // Reset back
            timeUntilFire = 0f;
        }
    }

    /// <summary>
    /// This method slows enemies using a raycast array.
    /// It raycast hits enemies within a circle area then gets the enemyMovement component of the hit enemy
    /// It then Updates their speed, slowing them down.
    /// It then starts a interval which is the total time that the enemy is slowed for.
    /// Once the interval finishes, the enemies speed resets to normal.
    /// </summary>
    private void SlowEnemies() 
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2) transform.position, 0f, enemyMask);

        if (hits.Length > 0) 
        {
            for(int i = 0; i < hits.Length; i++) 
            {
                RaycastHit2D hit = hits[i];

                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                em.UpdateSpeed(em.moveSpeed * 0.20f);

                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }

    /// <summary>
    /// This function resets the enemies speed after the interval "slowTime" has been completed
    /// </summary>
    /// <param name="em"></param>
    /// <returns></returns>
    private IEnumerator ResetEnemySpeed(EnemyMovement em) 
    {
        yield return new WaitForSeconds(slowTime);

        em.ResetSpeed();
    }
}
