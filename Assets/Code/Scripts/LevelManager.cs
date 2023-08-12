using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main; // This allows is to create a LevelManager class with specific properties
    public Transform[] startPoint; // This will allow me to set a startpoint
    public Transform[] path; // This allows me to set the enemy path

    public int currency;
    public int healthPoints;

    private void Awake()
    {
        main = this;
    }

    private void Start() { // sets currency and health points to 100 upon pressing play
        currency = 500;
        healthPoints = 100;
    }

    public void IncreaseCurrency(int amount) { // Increase currency by the amount in the parameter
        currency += amount;
    }

    public bool SpendCurrency(int amount) { // This is coding set up for the Tower Shop
        if (amount <= currency) {
            currency -= amount;
            return true;
        } else {
            Debug.Log("You do not have enough Stariam to purchase this item");
            return false;
        }
    }
    
}
