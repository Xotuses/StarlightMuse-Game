using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main; // This allows me to create a LevelManager class with specific properties
    public Transform[] startPoint; // This will allow me to set a startpoint
    public Transform[] path; // This allows me to set the enemy path

    public int currency;
    public static int healthPoints;

    private void Awake()
    {
        main = this;
    }

    // sets currency and health points to 100 upon pressing play
    private void Start() 
    { 
        currency = 4800;
        healthPoints = 100;
    }

    // Increase currency by the amount in the parameter
    public void IncreaseCurrency(int amount) 
    { 
        currency += amount;
    }

    /// <summary>
    /// This method takes the amount (cost) of a tower and deducts it from the players currency
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public void SpendCurrency(int amount) // This function takes a value from the players amount 
    { 
        if (amount <= currency) 
        {
            currency -= amount;
        } 
        else 
        {
            Debug.Log("You do not have enough Stariam to purchase this item");
        }
    }
    
}
