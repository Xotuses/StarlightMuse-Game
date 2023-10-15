using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    #region Serialized Fields
    [Header("Attributes")]
    // Sets amount of enemies
    [SerializeField] private int baseEnemies;
    // Sets amount of Boss enemies
    [SerializeField] private float baseBossEnemies; 
    // Sets amount of Sub Boss enemies
    [SerializeField] private int baseSubBossEnemies;
    // Sets speed in which enemies spawn
    [SerializeField] private float enemiesPerSecond; 

    // Sets Prep time 
    [SerializeField] private float timeBetweenWaves;
    // Quicker or more enemies that spawn
    [SerializeField] private float difficultyScalingFactor; 

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new();

    // Enemy lists for creating percentages for specfic enemy spawning
    [Header("Enemy Lists")] 
    [SerializeField] private List<GameObject> NormalList;
    [SerializeField] private List<GameObject> AbnormalList;
    [SerializeField] private List<GameObject> SpeedyList;
    [SerializeField] private List<GameObject> TankList;
    [SerializeField] private List<GameObject> SubBossList;
    [SerializeField] private List<GameObject> BossList;

    // Used to add more objects to the lists, increasing percentage to spawn the desired enemy
    [Header("Enemy Types")] 
    [SerializeField] private GameObject Normal;
    [SerializeField] private GameObject Abnormal;
    [SerializeField] private GameObject Speedy;
    [SerializeField] private GameObject Tank;

    [Header("Sub Boss Types")] 
    [SerializeField] private GameObject HighCut;
    [SerializeField] private GameObject BandPass;
    [SerializeField] private GameObject LowCut;
    #endregion

    public static int currentWave = 40;
    public static int enemiesAlive;
    public static int killCount;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;
    private bool isMultipleOfTen;
    private bool isMultipleOfForty;
    private float timeSinceLastSpawn;


    GameObject[] WaveArray;
    GameObject[] WaveArrayForConcat1;
    GameObject[] WaveArrayForConcat2;

    /// <summary>
    /// This functions ends the wave and changes conditions
    /// It then starts the next wave.
    /// </summary>
    public void EndWave() 
    {
        isSpawning = false;

        timeSinceLastSpawn = 0f;

        currentWave++;

        StartCoroutine(StartWave());
    }

    /// <summary>
    /// This adds a listener for the event onEnemyDestroy.
    /// Once the enemy is destroyed, the EnemyDestroyed function is triggered
    /// </summary>
    private void Awake()
    {
        onEnemyDestroy?.AddListener(EnemyDestroyed);
    }

    /// <summary>
    /// This executes the start wave function upon entering the level.
    /// </summary>
    private void Start()
    {
        StartCoroutine(StartWave()); 
    }

    /// <summary>
    /// This method tracks real time, then spawns enemies based on how much real time has elapsed.
    /// It then keeps doing this until there are no enemies left to spawn.
    /// Then it triggers the EndWave function.
    /// </summary>
    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        // This is the interval in which enemies spawn, 1 / 0.5 = 2 so the interval is 2 seconds.
        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0) 
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

    /// <summary>
    /// This is the functions that tells the level that an enemy has been destroyed
    /// "Drops enemy counter by one once an enemy is killed"
    /// </summary>
    private void EnemyDestroyed() 
    {
        enemiesAlive--;
        killCount++;
    }

    /// <summary>
    /// This method waits a set amount of time.
    /// Then it starts spawning. It also checks if the wave is a multiple of ten or forty.
    /// It assigns how many enemies will spawn this wave.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves); // Interval before the wave starts

        isSpawning = true;
        EnemyPercentageToSpawn(currentWave);
        CheckIfMultipleOfTenAndForty();
        enemiesLeftToSpawn = EnemiesPerWave;
        Debug.Log(EnemiesPerWave);
    }

    /// <summary>
    /// This method spawns enemies depending on the conditions that the player is in.
    /// It spawns enemies by filling an array with enemy prefabs of different types
    /// The more of one enemy type in an array, the higher chance of it spawning.
    /// It then takes a random number usuing a random range from 0 to the length of the array.
    /// Enemy types change depending on the currentWave.
    /// The method also allocates a start point for the enemies to spawn.
    /// </summary>
    private void SpawnEnemy()
    {
        GameObject prefabToSpawn = null;

        if (currentWave >= 1 && currentWave <= 40)
        {
            if (!isMultipleOfTen && !isMultipleOfForty)
            {
                prefabToSpawn = StandardEnemyConditions();
            }
            else if (isMultipleOfTen)
            {
                // Changes enemy types to sub boss enemys
                WaveArray = SubBossList.ToArray(); 

                int index = Random.Range(0, WaveArray.Length);
                prefabToSpawn = WaveArray[index];
            }
            else if (isMultipleOfForty)
            {
                // Changes enemy types to boss enemys
                WaveArray = BossList.ToArray(); 

                int index = Random.Range(0, WaveArray.Length);
                prefabToSpawn = WaveArray[index];
            }
        }

        else if (currentWave > 40)
        {
            // Combines all standard enemy types into a single array
            WaveArrayForConcat1 = NormalList.Concat(SpeedyList).ToArray();
            WaveArrayForConcat2 = AbnormalList.Concat(TankList).ToArray();
            WaveArray = WaveArrayForConcat1.Concat(WaveArrayForConcat2).ToArray();

            int index = Random.Range(0, WaveArray.Length);
            prefabToSpawn = WaveArray[index];
        }

        // Start point to null
        Transform sp = null;
        
        // Allocates start point
        foreach (var transform in LevelManager.main.startPoint) 
        {
            sp = transform;
        }
        
        // Spawns enemy
        Instantiate(prefabToSpawn, sp.position, Quaternion.identity); 
    }

    /// <summary>
    /// This function returns the enemy that will be spawned
    /// It does this by combining lists together and converting them into arrays to create higher chances of spawning specifc enemies.
    /// The enemy types depend on what wave the player is on.
    /// </summary>
    /// <returns> The prefab to spawn </returns>
    private GameObject StandardEnemyConditions()
    {
        GameObject prefabToSpawn;
        // Combines Arrays, 50/50 chance to spawn Normal or Speedy
        WaveArray = NormalList.Concat(SpeedyList).ToArray();

        // Creates an index that pulls a random number from the WaveArray
        int index = Random.Range(0, WaveArray.Length);

        // Selected number is chosen as the next enemy to spawn
        prefabToSpawn = WaveArray[index];

        if (currentWave >= 11 && currentWave <= 19)
        {
            WaveArrayForConcat1 = NormalList.Concat(SpeedyList).ToArray();

            // Introduces Abnormal enemy type to enemy pool
            WaveArray = WaveArrayForConcat1.Concat(AbnormalList).ToArray();

            int index2 = Random.Range(0, WaveArray.Length);
            prefabToSpawn = WaveArray[index2];
        }

        if (currentWave >= 21)
        {
            // Combines all standard enemy types into a single array
            WaveArrayForConcat1 = NormalList.Concat(SpeedyList).ToArray();
            WaveArrayForConcat2 = AbnormalList.Concat(TankList).ToArray();
            WaveArray = WaveArrayForConcat1.Concat(WaveArrayForConcat2).ToArray();

            int index3 = Random.Range(0, WaveArray.Length);
            prefabToSpawn = WaveArray[index3];
        }

        return prefabToSpawn;
    }

    /// <summary>
    /// This method increases specific enemy spawn rates by adding their prefab to their list.
    /// </summary>
    /// <param name="currentWave"></param>
    private void EnemyPercentageToSpawn(int currentWave)
    {
        if (currentWave >= 1 && currentWave <= 5)
        {
            // Adds a Speedy enemy into the Speedy list, increasing chances of Speedy enemies spawning
            SpeedyList.Add(Speedy);
        }

        if (currentWave > 10 && currentWave < 20) 
        {
            // Adds a Abnormal enemy into the abnormal list, increasing chances of Abnormal enemies spawning
            AbnormalList.Add(Abnormal); 
        }

        if (currentWave > 30 && currentWave < 40)
        {
            // Adds a Tank enemy into the Tank list, increasing chances of Tank enemies spawning
            TankList.Add(Tank);
        }

        if (currentWave == 20)
        {
            // Creates a variable that contains the current amount of Sub Bosses in the list
            var totalSubBosses = SubBossList.Count;
                
            for (int i = 0; i < totalSubBosses; i++) 
            {
                // Increases chances of Bandpasses spawning for every sub boss enemy in the list
                SubBossList.Add(BandPass);
            }
        }

        if (currentWave == 30)
        {
            var totalSubBosses = SubBossList.Count;

            for (int i = 0; i < totalSubBosses; i++) 
            {
                // Increases chances of LowCut spawning for every boss type in the list
                SubBossList.Add(LowCut);
            }
        }
    }

    /// <summary>
    /// This method checks if the current wave is a multiple of ten or forty.
    /// If it does, it changes the desired boolean value to true.
    /// </summary>
    private void CheckIfMultipleOfTenAndForty() 
    {
        isMultipleOfForty = currentWave % 40 == 0;

        if (!isMultipleOfForty)
        {
            isMultipleOfTen = currentWave % 10 == 0;
        }
    }

    /// <summary>
    /// This method creates the enemies that will appear on a specific wave.
    /// Each if statement uses a different math equation to calculate the value.
    /// </summary>
    private int EnemiesPerWave 
    {
        get
        {
            if (isMultipleOfTen)
            {
                return Mathf.RoundToInt(Mathf.Exp(difficultyScalingFactor) * Mathf.Log(currentWave));
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

