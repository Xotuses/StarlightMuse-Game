using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Rigidbody2D rb; // Allows me to move enemy

    [Header("Attributes")]
    [SerializeField] private float baseSpeed; // Adjusts Movement Speed

    public float moveSpeed;
    private Transform target; // Sets the point we want to move to.
    private int pathIndex = 0; // keeps target of the place on the path

    /// <summary>
    /// This method updates the speed of the enemy to the new speed (slowed speed)
    /// </summary>
    /// <param name="newSpeed"></param>
    public void UpdateSpeed(float newSpeed) // Changes moveSpeed to the default speed
    {
        moveSpeed = newSpeed;
    }

    /// <summary>
    /// This method resets the speed to base speed
    /// </summary>
    public void ResetSpeed() // Resets Speed to Default 
    {
        moveSpeed = baseSpeed;
    }
    
    /// <summary>
    /// This method assigns the move speed of every enemy to the base speed.
    /// (this is important for the bass blaster tower)
    /// It also makes it's target the first path point.
    /// </summary>
    private void Start() 
    {
        moveSpeed = baseSpeed;
        target = LevelManager.main.path[pathIndex]; 
    }

    /// <summary>
    /// Checks how far target postion is from transform position, if it is less or equal to 0.1.
    /// It then increases the pathIndex by 1, changing which point the enemy will go to.
    /// It also destorys the enemy if it reaches the end of the level.
    /// </summary>
    private void Update() 
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f) 
        {
            // Increase pathIndex by 1
            pathIndex++;

            // this statement destroys the enemy once it reaches the end
            if (pathIndex == LevelManager.main.path.Length) 
            { 
                EnemySpawner.onEnemyDestroy.Invoke();

                Destroy(gameObject);
                return;

            } 
            else 
            {
                // updates target
                target = LevelManager.main.path[pathIndex];  
            }
        }
    }

    /// <summary>
    /// This method moves the enemy onto the target.
    /// It also sets the enemies velocity using direction and speed.
    /// </summary>
    private void FixedUpdate() // Moves Rigidbody
    {
        // normalized means that the value only goes between 0 and 1
        Vector2 direction = (target.position - transform.position).normalized;

        // our player will move onto the next element
        rb.velocity = direction * moveSpeed; 
    }
}
