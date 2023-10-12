using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int baseEnemies; // Sets amount of enemies
    [SerializeField] private float baseBossEnemies; // Sets amount of Boss enemies
    [SerializeField] private int baseSubBossEnemies; // Sets amount of Sub Boss enemies
    [SerializeField] private float enemiesPerSecond; // Sets speed in which enemies spawn
    [SerializeField] private float timeBetweenWaves; // Sets Prep time 
    [SerializeField] private float difficultyScalingFactor; // Quicker or more enemies that spawn

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new();

    [Header("Enemy Lists")] // Enemy lists for creating percentages for specfic enemy spawning
    [SerializeField] private List<GameObject> NormalList;
    [SerializeField] private List<GameObject> AbnormalList;
    [SerializeField] private List<GameObject> SpeedyList;
    [SerializeField] private List<GameObject> TankList;
    [SerializeField] private List<GameObject> SubBossList;
    [SerializeField] private List<GameObject> BossList;

    [Header("Enemy Types")] // Used to add more objects to the lists, increasing percentage to spawn the desired enemy
    [SerializeField] private GameObject Normal;
    [SerializeField] private GameObject Abnormal;
    [SerializeField] private GameObject Speedy;
    [SerializeField] private GameObject Tank;

    [Header("Sub Boss Types")] 
    [SerializeField] private GameObject HighCut;
    [SerializeField] private GameObject BandPass;
    [SerializeField] private GameObject LowCut;

    public static int currentWave = 11;
    private float timeSinceLastSpawn;
    public static int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;
    private bool isMultipleOfTen;
    private bool isMultipleOfForty;

    GameObject[] WaveArray;
    GameObject[] WaveArrayForConcat1;
    GameObject[] WaveArrayForConcat2;

    private void Awake()
    {
        onEnemyDestroy?.AddListener(EnemyDestroyed);
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
        EnemyPercentageToSpawn(currentWave);
        CheckIfMultipleOfTenAndForty(); 
        enemiesLeftToSpawn = EnemiesPerWave;
        Debug.Log(EnemiesPerWave);
    }

    public void EndWave()
    {
        isSpawning = false;
            
        timeSinceLastSpawn = 0f;

        currentWave++;
        
        StartCoroutine(StartWave());
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

                if (currentWave >= 11 && currentWave <= 19)
                {
                    WaveArrayForConcat1 = NormalList.Concat(SpeedyList).ToArray(); // Combines Arrays, 50/50 chance to spawn Normal or Speedy
                    WaveArray = WaveArrayForConcat1.Concat(AbnormalList).ToArray(); // Introduces Abnormal enemy type to enemy pool

                    int index = Random.Range(0, WaveArray.Length);
                    prefabToSpawn = WaveArray[index];
                }

                if (isMultipleOfTen)
                {
                    WaveArray = SubBossList.ToArray(); // Changes enemy types to sub boss enemys

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

            if (currentWave > 40)
            {
                WaveArrayForConcat1 = NormalList.Concat(SpeedyList).ToArray();
                WaveArrayForConcat2 = AbnormalList.Concat(TankList).ToArray();
                WaveArray = WaveArrayForConcat1.Concat(WaveArrayForConcat2).ToArray(); // Concatinates all normal enemy types into the wave array

                int index = Random.Range(0, WaveArray.Length);
                prefabToSpawn = WaveArray[index];
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
                // Increase chances of speedy enemies spawning
            }

            if (currentWave > 10 && currentWave < 20) 
            {
                AbnormalList.Add(Abnormal); 
                // Increases chances of abnormals spawning
            }

            if (currentWave == 20)
            {
                var totalSubBosses = SubBossList.Count;
                
                for (int i = 0; i < totalSubBosses; i++) 
                {
                    SubBossList.Add(BandPass);
                    // Increases chances of Bandpasses spawning for every boss type in the list
                }
            }

            if (currentWave == 30)
            {
                var totalSubBosses = SubBossList.Count;

                for (int i = 0; i < totalSubBosses; i++) 
                {
                    SubBossList.Add(LowCut);
                    // Increases chances of LowCut spawning for every boss type in the list
                }
            }
        }
    }

    private void CheckIfMultipleOfTenAndForty() // Checks if the current wave is divisible by 10 or 40
    {
        if (currentWave % 40 == 0)
        {
            isMultipleOfForty = true;
        }
        else
        {
            isMultipleOfForty = false;
        }

        if (!isMultipleOfForty)
        {
            if (currentWave % 10 == 0)
            {
                isMultipleOfTen = true;
            }
            else
            {
                isMultipleOfTen = false;
            }
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

