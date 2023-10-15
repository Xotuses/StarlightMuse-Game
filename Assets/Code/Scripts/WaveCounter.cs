using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveCounter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI WaveValueUI = null;
    [SerializeField] TextMeshProUGUI LiveValueUI = null;

    private void OnGUI()
    {
        if (WaveValueUI != null && LiveValueUI != null)
        {
            // Displays Current Wave
            WaveValueUI.text = EnemySpawner.currentWave.ToString();

            // Displays Health Points
            LiveValueUI.text = LevelManager.healthPoints.ToString();
        }
    }
}
