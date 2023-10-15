using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SynthSweeper : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private Transform synthSweeperBody;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attribute")]
    [SerializeField] private float targetingRange;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float bps; // Bullets per Second

    private Transform target;
    private float timeUntilFire;

    /// <summary>
    /// This method operates the turret.
    /// It searches for the target if there is no target.
    /// It then rotates towards the target once found.
    /// It then checks if it is in range.
    /// It then tracks real time in the timeUntilFire variable
    /// It then shoots and resets the variable, shooting everytime 1f/bps matches the timeUntilFire variable.
    /// </summary>
    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget(); 

        if (!CheckTargetIsInRange()) 
        {
            target = null;
        } else {

            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bps) 
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    /// <summary>
    /// This method shoots a bullet.
    /// It does this by instantiating a bullet prefab at the firing point of the turret.
    /// It then sets the bullets target to the target being prioritised by the FindTarget method.
    /// </summary>
    private void Shoot() 
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    /// <summary>
    /// This method finds the target using a raycast.
    /// This is done by casting a ray in a circle area, this hits enemies once they enter the area.
    /// It then priortises the first enemy hit and makes it the target.
    /// </summary>
    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2) transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    /// <summary>
    /// This method checks if the target is within range of the towers targeting range.
    /// </summary>
    /// <returns> true or false </returns>
    private bool CheckTargetIsInRange() 
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
    // Calculates the direction vector from the SynthSweeper's position to the target's position.
    Vector3 direction = target.position - transform.position;

    // Calculates the angle in degrees based on the direction vector.
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    // Creates a quaternion representing the desired rotation around the Z-axis.
    Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

    // Applys the target rotation to the SynthSweeper's body.
    synthSweeperBody.rotation = Quaternion.RotateTowards(synthSweeperBody.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
