using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQ : MonoBehaviour
{
    [SerializeField] public Messenger messengerPrefab;
    [SerializeField] Unit activeUnit;
    Vector3 newDestinationMessage;

    public static Vector3 NULL_VECT;                // treating like a const
    
    [SerializeField] private List<Unit> unitList;
    [SerializeField] private List<Messenger> messengerList;
    [SerializeField] int unitListIndex = 0;

    private void Awake()
    {
        NULL_VECT = this.transform.position;        // use this position as 'null' value
        newDestinationMessage = NULL_VECT;

        messengerList = new List<Messenger>();
    }

    private void Start()
    {
        unitList = new List<Unit>(FindObjectsOfType<Unit>());
        activeUnit = unitList.ToArray()[unitListIndex];

        activeUnit.ActivateUnit(true);
        
        Interface.OnRightMouseButtonDown += Interface_OnRightMouseButtonDown;
        Interface.OnKeyDownN += Interface_OnKeyDownN;
    }

    void Interface_OnKeyDownN(object sender, EventArgs e)
    {
        unitListIndex = (unitListIndex + 1) % unitList.Count;
        activeUnit = unitList.ToArray()[unitListIndex];

        foreach (Unit unit in unitList)
        {
            if (unit == activeUnit)
            {
                unit.ActivateUnit(true);
            }
            else
            {
                unit.ActivateUnit(false);
            }
        }
    }

    void Interface_OnRightMouseButtonDown(object sender, Vector3 vector)
    {
        newDestinationMessage = vector;
    }

    private void FixedUpdate()
    {
        if (newDestinationMessage != NULL_VECT)
        {
            // instantiate messenger
            Messenger messenger = Instantiate(messengerPrefab, this.transform);

            // add new messenger to messenger list
            messengerList.Add(messenger);

            // setup messenger with reference to this HQ, the message, and the active unit
            messenger.SetupMessenger(this, newDestinationMessage, activeUnit);

            // set messenger destination as active unit
            messenger.SetDestination(activeUnit.transform.position);

            // set newDestination as 'null'
            newDestinationMessage = NULL_VECT;
        }
    }


    public void RemoveMessengerFromList(Messenger messenger)
    {
        if (messengerList.Contains(messenger))
        {
            messengerList.Remove(messenger);
            Destroy(messenger.gameObject);
        }
        else
        {
            Debug.LogError("Trying to destroy messenger not in messengerList");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
    }
}
