using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointCollision : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    
    private void OnCollisionEnter2D(Collision2D other) 
    {
        Debug.Log("Collided");
        other.gameObject.GetComponent<Health>().healthDamage();
    }
}
