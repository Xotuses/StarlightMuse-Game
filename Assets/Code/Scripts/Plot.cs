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

    private void Start() 
    {
        startColor = sr.color;
    }

    private void OnMouseEnter() 
    {
        if (!PauseMenu.IsPaused)
        {
            isMouseHovering = true;
            sr.color = hoverColor;
        }
    }

    private void OnMouseExit() 
    {
        if (!PauseMenu.IsPaused)
        {
            sr.color = startColor;
            isMouseHovering = false;
        }
    }

    private void OnMouseDown() 
    {
        if (!PauseMenu.IsPaused)
        {
            if (!Menu.isOnMenu)
            {
                if (tower != null) return;
                // If a tower is already on the plot, do not build another one

                Tower towerToBuild = BuildManager.main.GetSelectedTower();

                if (towerToBuild.cost > LevelManager.main.currency)
                {
                    Debug.Log("You can't afford this tower");
                    return;
                }

                LevelManager.main.SpendCurrency(towerToBuild.cost);
                // This will take the currency from the Level Manager

                assignedRefundCost = towerToBuild.refundCost;
                // Assigns the refund cost to the "About to be placed" tower

                tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
                // If the player meets all requirements, instantiate the towerToBuild
            }
        }           
    }

    private void Update() // This destroys tower on plot if there is a tower on hovered plot and refunds some of the spent currency
    {
        if (!PauseMenu.IsPaused)
        {
            if (isMouseHovering)
            {
                if (Input.GetKeyDown(KeyCode.D) && tower != null)
                {
                    LevelManager.main.IncreaseCurrency(assignedRefundCost);
                    Destroy(tower);
                    tower = null;
                }
            }
        }

        if (PauseMenu.IsPaused) // Makes all plots the start color when paused
        {
            sr.color = startColor;
        }
    }
}