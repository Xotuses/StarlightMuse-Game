using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveCounter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI WaveValueUI;

    private void OnGUI() {
        WaveValueUI.text = EnemySpawner.currentWave.ToString();
    }
}
