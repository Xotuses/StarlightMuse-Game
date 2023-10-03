using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8; // Sets amount of enemies
    [SerializeField] private float baseBossEnemies = 0.5f; // Sets amount of Boss enemies
    [SerializeField] private int baseSubBossEnemies = 1; // Sets amount of Sub Boss enemies
    [SerializeField] private float enemiesPerSecond = 0.5f; // Sets speed in which enemies spawn
    [SerializeField] private float timeBetweenWaves = 5f; // Sets Prep time 
    [SerializeField] private float difficultyScalingFactor = 0.75f; // Quicker or more enemies that spawn

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new();

    [Header("Enemy Lists")] // Enemy lists for creating percentages for specfic enemy spawning
    [SerializeField] private List<GameObject> NormalList;
    [SerializeField] private List<GameObject> AbnormalList;
    [SerializeField] private List<GameObject> SpeedyList;
    [SerializeField] private List<GameObject> TankList;
    [SerializeField] private List<GameObject> SubBoss1List;
    [SerializeField] private List<GameObject> BossList;

    [Header("Enemy Types")] // Used to add more objects to the lists, increasing percentage to spawn the desired enemy
    [SerializeField] private GameObject Normal;
    [SerializeField] private GameObject Abnormal;
    [SerializeField] private GameObject Speedy;
    [SerializeField] private GameObject Tank;

    public static int currentWave = 40;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;
    GameObject[] WaveArray;
    private bool isMultipleOfTen;
    private bool isMultipleOfForty;
    
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
        yield return new WaitForSeconds(timeBetweenWaves); // Interval before the wave starts

        isSpawning = true;
        CheckIfMultipleOfTenAndForty(); 
        enemiesLeftToSpawn = EnemiesPerWave;
    }

    public void EndWave()
    {
        isSpawning = false;
            
        timeSinceLastSpawn = 0f;

        currentWave++;
        
        StartCoroutine(StartWave());

        EnemyPercentageToSpawn(currentWave);

        string result = "Array contents: "; // Shows me what was inside the concatinated array (debugging purposes)
        foreach (var item in WaveArray)
        {
            result += item.ToString() + ", ";
        }
        Debug.Log(result);
    }

    private void SpawnEnemy() // This is the function containing all the conditions to spawn specific arrays depending on the wave
    {
        GameObject prefabToSpawn = null;

        if (currentWave != 0) 
        {
            if (currentWave >= 1 && currentWave <= 40)
            {
                if (!isMultipleOfTen && !isMultipleOfForty)
                {
                    WaveArray = NormalList.Concat(SpeedyList).ToArray(); // Combines Arrays, 50/50 chance to spawn Normal or Speedy

                    int index = Random.Range(0, WaveArray.Length);
                    prefabToSpawn = WaveArray[index];
                }

                if (isMultipleOfTen)
                {
                    WaveArray = SubBoss1List.ToArray(); // Changes enemy types to sub boss enemys

                    int index = Random.Range(0, WaveArray.Length);
                    prefabToSpawn = WaveArray[index];
                }

                if (isMultipleOfForty)
                {
                    WaveArray = BossList.ToArray(); // Changes enemy types to boss enemys

                    int index = Random.Range(0, WaveArray.Length);
                    prefabToSpawn = WaveArray[index];
                }
            }
        }

        Transform sp = null; // Start point to null

        foreach (var transform in LevelManager.main.startPoint) // Allocates start point
        {
            sp = transform;
        }

        Instantiate(prefabToSpawn, sp.position, Quaternion.identity); // Spawns enemy
    }

    private void EnemyPercentageToSpawn(int currentWave)
    {
        if (currentWave != 0)
        {
            if (currentWave <= 5)
            {
                SpeedyList.Add(Speedy);
            }

            if (currentWave < 10 && currentWave > 6) 
            {
                AbnormalList.Add(Abnormal);
            }
        }
    }

    private void CheckIfMultipleOfTenAndForty() // Checks if the current wave is divisible by 10 or 40
    {
        if (currentWave % 10 == 0)
        {
            isMultipleOfTen = true;
        } 
        else
        {
            isMultipleOfTen = false;
        }

        if (currentWave % 40 == 0)
        {
            isMultipleOfForty = true;
        }
        else
        {
            isMultipleOfForty = false;
        }
    }

    private int EnemiesPerWave // This code block produces the number of enemies that will be spawned
    {
        get
        {
            if (isMultipleOfTen)
            {
                return Mathf.RoundToInt(Mathf.Exp(difficultyScalingFactor) * baseSubBossEnemies);
            }
            else if (isMultipleOfForty)
            {
                return Mathf.RoundToInt(Mathf.Exp(difficultyScalingFactor) * baseBossEnemies);
            }
            else
            {
                return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
            }
        }
    }
}

