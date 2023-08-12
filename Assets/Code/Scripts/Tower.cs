using System;
using UnityEngine;

[Serializable]
public class Tower
{
    public string name; 
    // Name of tower

    public int cost;
    // amount of currency needed

    public int refundCost;
    // How much the player recieves on deletion

    public GameObject prefab;
    // The gameObject in question

    public Tower (string _name, int _cost, int _refundCost, GameObject _prefab) {
        name = _name;
        cost = _cost;
        refundCost = _refundCost;
        prefab = _prefab;
    }
    // This assigns the variables to their respective values and allows the Tower class to be present within the shop
}
