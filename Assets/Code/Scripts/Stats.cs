using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI TotalHealthUI = null;
    [SerializeField] TextMeshProUGUI TotalKillsUI = null;

    private void OnGUI()
    {
        if (TotalHealthUI != null && TotalKillsUI != null)
        {
            // Displays Total Health left
            TotalHealthUI.text = LevelManager.healthPoints.ToString();

            // Displays the amount of enemies killed
            TotalKillsUI.text = EnemySpawner.killCount.ToString();
        }
    }
}
