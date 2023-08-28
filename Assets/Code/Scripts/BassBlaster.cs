using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BassBlaster : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float targetingRange = 2f;
    [SerializeField] private float aps = 4f; // Bullets per Second
    [SerializeField] private float slowTime = 1.5f;

    [Header("Misc")]
    [SerializeField] private LayerMask enemyMask;
    
    private Transform target;
    private float timeUntilFire;

    // New Variant of Bass Blaster

    private void Update() {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / aps) 
        {
            SlowEnemies();
            timeUntilFire = 0f;
        }
    }

    private void SlowEnemies() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2) transform.position, 0f, enemyMask);

        if (hits.Length > 0) {
            for(int i = 0; i < hits.Length; i++) {
                RaycastHit2D hit = hits[i];

                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                FindObjectOfType<AudioManager>().Play("BassBlast");
                em.UpdateSpeed(em.moveSpeed * 0.20f);

                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em) {
        yield return new WaitForSeconds(slowTime);

        em.ResetSpeed();
    }

    // Misc Code

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
