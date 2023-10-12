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
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bps = 1f; // Bullets per Second

    private Transform target;
    private float timeUntilFire;

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget(); // This rotates the object towards the target

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

    private void Shoot() 
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2) transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

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
