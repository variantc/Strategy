using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : BaseUnit
{
    Material defaultMaterial;
    [SerializeField] Material activeMaterial;
    [SerializeField] float SIGHT_RADIUS = 10f;

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

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= messengerTime)
        {
            SendMessengerToHQ();
            timer = 0f;
        }
    }

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
                SetDestination(messenger.GetMessage());

                // we also want to trigger the unit to do a sphere overlap
                // and report the enemy unit positions to the messenger
                List<Vector3> enemyPosList = new List<Vector3>();
                Collider[] colliders = Physics.OverlapSphere(this.transform.position, SIGHT_RADIUS);
                if (colliders.Length <= 0)
                {
                    return;
                }
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject.TryGetComponent(out EnemyUnit enemyUnit))
                    {
                        enemyPosList.Add(enemyUnit.transform.position);
                    }
                }

                messenger.SetKnownEnemyPosList(enemyPosList);
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
