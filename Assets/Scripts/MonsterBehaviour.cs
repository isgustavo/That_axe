using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class MonsterBehaviour : MonoBehaviour {

    protected NavMeshAgent agent;
    [SerializeField]
    protected Transform playerPosition;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(playerPosition.position);
    }
}
