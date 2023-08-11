using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveCounter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI WaveValueUI;
    [SerializeField] TextMeshProUGUI LiveValueUI;

    private void OnGUI() {
        WaveValueUI.text = EnemySpawner.currentWave.ToString();
        LiveValueUI.text = LevelManager.main.healthPoints.ToString();
    }
}
