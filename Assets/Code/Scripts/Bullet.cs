using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;
    [SerializeField] public float bulletLifespan = 4f; // This is the life span of the bullet

    private Transform target;

    public void SetTarget(Transform _target) {
        target = _target;
    }

    private void FixedUpdate() 
    {
        if (!target) return;
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * bulletSpeed; 
        // If the target moves position, it will recalculate the direction and times that by bps
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        other.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);
        Destroy(gameObject);
        // CHANGES
        Debug.Log("Bullet Destroyed!");
    }

    void Start() {
        // Starts Coroutine 
        StartCoroutine(DestroyAfterLifespan()); 
    }

    IEnumerator DestroyAfterLifespan() {
        
        // This waits for bulletLifespan seconds
        yield return new WaitForSeconds(bulletLifespan);

        // Destroy the bullet gameObject
        Destroy(gameObject);
    }
}
