using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointCollision : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    
    /// <summary>
    /// This method damages the player once an enemy has reached the end of the level.
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other) 
    {
        other.gameObject.GetComponent<Health>().HealthDamage();
    }
}
