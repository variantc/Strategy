using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionUpdater : MonoBehaviour
{
    public static VisionUpdater Instance { get; private set; }

    [SerializeField] ShownEnemyUnit enemyUnitPrefab;
 
    private void Awake()
    {
        Instance = this;
    }

    public void ShowEnemyUnits(List<Vector3> enemyUnitPositionList)
    {
        // When the messenger returns with list of enemy positions spawn the 
        // enemy units' last known representation
        foreach (Vector3 pos in enemyUnitPositionList)
        {
            Instantiate(enemyUnitPrefab, pos, Quaternion.identity);
        }
    }
}