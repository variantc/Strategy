using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseUnit : MonoBehaviour
{
    protected NavMeshAgent navMeshAgent;
    protected MeshRenderer meshRenderer;

    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetDestination(Vector3 destination)
    {
        navMeshAgent.SetDestination(destination);
    }
}
