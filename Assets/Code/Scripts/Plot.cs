using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject tower;
    private Color startColor;

    private void Start() {
        startColor = sr.color;
    }

    private void OnMouseEnter() {
        sr.color = hoverColor;
    }

    private void OnMouseExit() {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
    if (tower != null) return; // If a tower is already on the plot, do not build another one

    GameObject towerToBuild = BuildManager.main.GetSelectedTower();
    // Check if the player has enough resources and meets other requirements to build the tower
    // Add your resource and requirement checks here

    // If the player meets all requirements, instantiate the towerToBuild
    tower = Instantiate(towerToBuild, transform.position, Quaternion.identity);
    }
}
