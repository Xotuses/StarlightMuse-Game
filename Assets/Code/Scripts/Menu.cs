using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] Animator anim;
    
    public static bool isOnMenu = true;
    private bool isMenuOpen = true;

    /// <summary>
    /// This method triggers the animation for the menu sidebar
    /// Both values are true as the menu opens upon startup of the script
    /// Once the player presses the shop button, it changes the boolean values.
    /// This triggers the animation to close the menu sidebar
    /// </summary>
    public void ToggleMenu() 
    {
        isMenuOpen = !isMenuOpen;
        
        isOnMenu = !isOnMenu;
        
        anim.SetBool("MenuOpen", isMenuOpen && isOnMenu);
        
    }

    private void OnGUI() 
    {
        // Displays currency on Menu
        currencyUI.text = LevelManager.main.currency.ToString();
    }
}
