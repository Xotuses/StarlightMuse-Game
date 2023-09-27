using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8; // Sets amount of enemies
    [SerializeField] private float enemiesPerSecond = 0.5f; // Sets speed in which enemies spawn
    [SerializeField] private float timeBetweenWaves = 5f; // Sets Prep time 
    [SerializeField] private float difficultyScalingFactor = 0.75f; // Quicker or more enemies that spawn
    [SerializeField] private int waveDifficultyValue; // Difficulty Values for waves determine the level of enemies that will spawn

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    [Header("Enemy Lists")]
    [SerializeField] private List<GameObject> NormalList;
    [SerializeField] private List<GameObject> AbnormalList;
    [SerializeField] private List<GameObject> SpeedyList;
    [SerializeField] private List<GameObject> TankList;

    [Header("Enemy Types")]
    [SerializeField] private GameObject Normal;
    [SerializeField] private GameObject Abnormal;
    [SerializeField] private GameObject Speedy;
    [SerializeField] private GameObject Tank;

    public static int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;
    GameObject[] WaveArray;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0) // This is the interval in which enemies spawn, 1 / 0.5 = 2 so the interval is 2 seconds.
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
    }

    public void EndWave()
    {
        isSpawning = false;
            
        timeSinceLastSpawn = 0f;

        currentWave++;
        
        StartCoroutine(StartWave());

        DifficultyScaling();

        string result = "Array contents: "; // Shows me what was inside the concatinated array (debugging purposes)
        foreach (var item in WaveArray)
        {
            result += item.ToString() + ", ";
        }
        Debug.Log(result);
    }

    private void SpawnEnemy()
    {
        GameObject prefabToSpawn = null;

        if (currentWave != 0) // These are the conditions to spawn specific arrays depending on the wave
        {
            WaveArray = NormalList.Concat(SpeedyList).ToArray(); // Combines Arrays, 50/50 chance to spawn Normal or Speedy

            int index = Random.Range(0, WaveArray.Length);
            prefabToSpawn = WaveArray[index];
        }

        Transform sp = null; // Start point to null

        foreach (var transform in LevelManager.main.startPoint) // Allocates start point
        {
            sp = transform;
        }

        Instantiate(prefabToSpawn, sp.position, Quaternion.identity); // Spawns enemy
    }

    private void DifficultyScaling()
    {
        if (currentWave != 0)
        {
            NormalList.Add(Normal);
            SpeedyList.Add(Speedy);
        }
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }
}

