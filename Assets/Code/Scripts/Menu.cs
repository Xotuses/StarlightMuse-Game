using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] Animator anim;

    private bool isMenuOpen = true;

    public void ToggleMenu() {
        isMenuOpen = !isMenuOpen;
        // Defines Boolean Value

        anim.SetBool("MenuOpen", isMenuOpen);
        // Reassigns Boolean Value based on animation
    }

    private void OnGUI() {
        currencyUI.text = LevelManager.main.currency.ToString();
    }
}
