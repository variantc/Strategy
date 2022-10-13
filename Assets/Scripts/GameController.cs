using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameController Instance { get; private set; }

    // List of all the units
    private List<Unit> unitList;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        unitList = new List<Unit>(FindObjectsOfType<Unit>());
    }
}
