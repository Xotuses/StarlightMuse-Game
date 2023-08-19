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
    
    private void Start() {
        moveSpeed = baseSpeed;
        target = LevelManager.main.path[pathIndex]; 
    }

    private void Update() { // this will check the target 
        if (Vector2.Distance(target.position, transform.position) <= 0.1f) { // Checks how far target postion is from transform position, if it is less or equal to 0.1.
            pathIndex++; // Increase pathIndex by 1

            if (pathIndex == LevelManager.main.path.Length) { // this statement destroys the enemy once it reaches the end
                EnemySpawner.onEnemyDestroy.Invoke();
                // LevelManager.main.healthPoints -= Health.hitPoints;
                Destroy(gameObject);
                return;
            } else {
                target = LevelManager.main.path[pathIndex]; // updates target 
            }
        }
    }

    private void FixedUpdate() { // Moves Rigidbody  
        Vector2 direction = (target.position - transform.position).normalized;
        // normalized means that the value only goes between 0 and 1
        
        rb.velocity = direction * moveSpeed; // our player will move onto the next element
    }

    public void UpdateSpeed(float newSpeed) { // Changes moveSpeed to the d
        moveSpeed = newSpeed; 
    }

    public void ResetSpeed() { // Resets Speed to Default 
        moveSpeed = baseSpeed;
    }
}
