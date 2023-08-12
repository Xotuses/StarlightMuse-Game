using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject tower = null;
    private Color startColor;
    private bool isMouseHovering = false;
    private int assignedRefundCost;

    private void Start() {
        startColor = sr.color;
    }

    private void OnMouseEnter() {
        isMouseHovering = true;
        sr.color = hoverColor;
    }

    private void OnMouseExit() {
        sr.color = startColor;
        isMouseHovering = false;
    }

    private void OnMouseDown() {
        if (!Menu.isOnMenu) {
            if (tower != null) return; // If a tower is already on the plot, do not build another one

            Tower towerToBuild = BuildManager.main.GetSelectedTower();

            if (towerToBuild.cost > LevelManager.main.currency) {
            Debug.Log("You can't afford this tower");
            return;
            }

            LevelManager.main.SpendCurrency(towerToBuild.cost); 
            // This will take the currency from the Level Manager

            assignedRefundCost = towerToBuild.refundCost;

            tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
            // If the player meets all requirements, instantiate the towerToBuild
        }           
    }

    private void Update() { // This destroys tower on plot if there is a tower on hovered plot and refunds some of the spent currency
        if (isMouseHovering) {
            if (Input.GetKeyDown(KeyCode.D) && tower != null) {
                LevelManager.main.IncreaseCurrency(assignedRefundCost);
                Destroy(tower);
                tower = null;
            }
        }
    }
}