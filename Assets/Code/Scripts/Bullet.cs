using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Attributes")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int bulletDamage;
    public float bulletLifespan; // This is the life span of the bullet

    private Transform target;

    /// <summary>
    /// This method sets the target for the bullet.
    /// </summary>
    /// <param name="_target"></param>
    public void SetTarget(Transform _target) 
    {
        target = _target;
    }

    /// <summary>
    /// This method allows the bullet to travel towards the target.
    /// </summary>
    private void FixedUpdate() 
    {
        // If the target moves position, it will recalculate the direction and times that by bps
        if (!target) return;
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * bulletSpeed; 
        
    }

    /// <summary>
    /// On collision, this method allows enemies to take damage from bullets that hit them.
    /// It then destroys the bullet once it has hit the enemy.
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other) 
    {
        other.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);
        Destroy(gameObject);
    }

    /// <summary>
    /// This starts a coroutine which destroys the bullet after an interval
    /// </summary>
    void Start() 
    {
        // Starts Coroutine 
        StartCoroutine(DestroyAfterLifespan()); 
    }

    /// <summary>
    /// This method waits for the bulletLifeSpan to run out.
    /// It then destroys the bullet
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyAfterLifespan() {
        
        // This waits for bulletLifespan seconds
        yield return new WaitForSeconds(bulletLifespan);

        // Destroy the bullet gameObject
        Destroy(gameObject);
    }
}
