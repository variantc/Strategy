using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Messenger : BaseUnit
{
    [SerializeField] HQ hQ;
    [SerializeField] Vector3 message;
    [SerializeField] Unit unit;

    // Do we want both EnemyUnit and Vector3 Lists?
    [SerializeField] List<EnemyUnit> knownEnemyList;

    // To store known enemy list
    [SerializeField] List<Vector3> knownEnemyPosList;

    [SerializeField] float SIGHT_RADIUS = 10f;
    private Vector3 thisPosition;
    private Vector3 lastPosition;

    protected override void Awake()
    {
        base.Awake();
        thisPosition = this.transform.position;
        lastPosition = this.transform.position;
    }

    public void SetupMessenger(HQ hQ, Vector3 message, Unit unit, bool IsFromUnit=false)
    {
        this.hQ = hQ;
        this.unit = unit;
        this.message = message;

        if (IsFromUnit)
        {
            navMeshAgent.SetDestination(hQ.transform.position);
        }
        else
        {
            // Set destination to current unit position
            // ultimately last known position?
            navMeshAgent.SetDestination(unit.transform.position);
        }
    }

    private void FixedUpdate()
    {
        // grab current position
        thisPosition = this.transform.position;

        // if we've dropped off the message, check if we have arrived at HQ
        if (message == HQ.NULL_VECT)
            CheckReturnedToHQ();

        // Check if messenger is stationary
        if ((thisPosition - lastPosition).magnitude == 0)
        {
            // check if can see our unit and if not, head back to hQ
            if (!CheckUnitInView())
                navMeshAgent.SetDestination(hQ.transform.position);
        }

        // store current position for next loop
        lastPosition = thisPosition;
    }

    bool CheckUnitInView()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, SIGHT_RADIUS);
        if (colliders.Length <= 0)
        {
            return false;
        }
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out Unit unit))
            {
                if (this.unit == unit)
                {
                    // We see our unit - head to new location
                    navMeshAgent.SetDestination(unit.transform.position);
                    return true;
                }
            }
        }
        return false;
    }

    void CheckReturnedToHQ()
    {
        float arrivedDistance = 0.5f;

        if ((this.transform.position - hQ.transform.position).magnitude < arrivedDistance)
        {
            // When returns to HQ tell VisionUpdater to show these units
            VisionUpdater.Instance.ShowEnemyUnits(knownEnemyPosList);

            hQ.RemoveMessengerFromList(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckForTargetUnit(other);
    }

    void CheckForTargetUnit(Collider collider)
    {
        if (collider.TryGetComponent(out Unit unit))
        {
            if (unit == this.unit)
            {
                // Give message to Unit
                this.message = HQ.NULL_VECT;

                // Now get the unit to do a sphere overlap and report to this messenger
                // the locations of the enemy units

                //    knownEnemyPosList

                navMeshAgent.SetDestination(this.hQ.transform.position);
                
                this.unit = null;

            }
            else
                Debug.Log(unit + " - " + this.unit);
        }
    }

    public void SetKnownEnemyPosList(List<Vector3> enemyPosList) { knownEnemyPosList = enemyPosList; }

    public Vector3 GetMessage() => message;
    public Unit GetTargetUnit() => unit;
}
