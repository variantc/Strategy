using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : BaseUnit
{
    private Material defaultMaterial;
    [SerializeField] Material activeMaterial;

    [SerializeField] HQ hQ;

    bool IsSelected = false;

    float timer = 0f;
    float messengerTime = 1f;

    protected override void Awake()
    {
        base.Awake();
        defaultMaterial = meshRenderer.material;
    }

    private void Start()
    {
        // TEMPORARY
        hQ = FindObjectOfType<HQ>();
    }

    //protected override void FixedUpdate() 
    //{
    //    base.FixedUpdate();
    //    Debug.Log(knownEnemyList.Count + ", " + knownEnemyPosList.Count);
    //}

    void SendMessengerToHQ()
    {
        Messenger messenger = Instantiate(hQ.messengerPrefab, this.transform.position, Quaternion.identity);
        messenger.SetupMessenger(hQ, HQ.NULL_VECT, this, true);
    }

    void CheckInSightRange()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Messenger messenger))
        {
            // Check if the messenger has one
            if (messenger.GetMessage() == HQ.NULL_VECT)
                return;

            // Check it's a message for this unit
            if (messenger.GetTargetUnit() == this)
            {
                // Head to messenger's delivered target
                navMeshAgent.SetDestination(messenger.GetMessage());

                // report the enemy unit positions to the messenger
                messenger.AddToKnownEnemyDict(knownEnemyDict);
            }
        }
    }

    public void ActivateUnit(bool activate)
    {
        IsSelected = activate;

        if (activate)
        {
            meshRenderer.material = activeMaterial;
        }
        else
        {
            meshRenderer.material = defaultMaterial;
        }
    }
}
