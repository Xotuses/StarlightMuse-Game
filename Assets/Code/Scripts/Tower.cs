using System;
using UnityEngine;

[Serializable]
public class Tower
{
    public string name; 
    // Name of tower

    public int cost;
    // amount of currency needed

    public GameObject prefab;
    // The gameObject in question

    public Tower (string _name, int _cost, GameObject _prefab) {
        name = _name;
        cost = _cost;
        prefab = _prefab;
    }
    // This assigns the variables to their respective values and allows the Tower class to be present within the shop
}
