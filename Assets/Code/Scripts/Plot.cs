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

    // Assigns the default color to the plot 
    private void Start()
    {
        startColor = sr.color;
    }

    // Changes color once the mouse hovers over the plot
    private void OnMouseEnter() 
    {
        if (!PauseMenu.IsPaused && !ConditionsForGameplay.isOnVictoryScreen)
        {
            isMouseHovering = true;
            sr.color = hoverColor;
        }
    }
    
    // Changes color if once mouse has stopped hovering over the plot
    private void OnMouseExit() 
    {
        if (!PauseMenu.IsPaused && !ConditionsForGameplay.isOnVictoryScreen)
        {
            sr.color = startColor;
            isMouseHovering = false;
        }
    }

    /// <summary>
    /// This method allows the player to place a tower if conditions are met.
    /// The tower that is going to be built is assigned via the menu sidebar.
    /// The method then spends the players currency, assigns a refund cost to the placed tower
    /// Then instantiates the tower on the specfic plot.
    /// </summary>
    private void OnMouseDown() 
    {
        if (!PauseMenu.IsPaused && !ConditionsForGameplay.isOnVictoryScreen)
        {
            if (!Menu.isOnMenu)
            {
                // If a tower is already on the plot, do not build another one
                if (tower != null) 
                    return;

                Tower towerToBuild = BuildManager.main.GetSelectedTower();

                if (towerToBuild.cost > LevelManager.main.currency)
                {
                    Debug.Log("You can't afford this tower");
                    return;
                }

                // This will take the currency from the Level Manager
                LevelManager.main.SpendCurrency(towerToBuild.cost);

                // Assigns the refund cost to the "About to be placed" tower
                assignedRefundCost = towerToBuild.refundCost;

                // If the player meets all requirements, instantiate the towerToBuild
                tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
            }
        }           
    }

    /// <summary>
    /// This destroys tower on plot if there is a tower on hovered plot and refunds some of the spent currency.
    /// If there are no menus or screens present, if the mouse is hovering over a tower and the D key is pressed
    /// It will delete the tower, refunding currency.
    /// </summary>
    private void Update() 
    {
        if (!PauseMenu.IsPaused && !ConditionsForGameplay.isOnVictoryScreen)
        {
            if (isMouseHovering && (Input.GetKeyDown(KeyCode.D) && tower != null))
            {
                LevelManager.main.IncreaseCurrency(assignedRefundCost);
                Destroy(tower);
                tower = null;
            }
        }

        // Makes all plots the start color when paused
        if (PauseMenu.IsPaused) 
        {
            sr.color = startColor;
        }
    }
}