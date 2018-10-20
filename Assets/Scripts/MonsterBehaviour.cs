using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class MonsterBehaviour : MonoBehaviour, ISpawnebleObject
{

    protected NavMeshAgent agent;

    protected Transform player;

    public bool IsActiveInHierarchy()
    {
        return gameObject.activeInHierarchy;
    }

    public void SetStartPosition(Vector3 position)
    {
        Debug.Log(position);
        transform.position = position;
    }

    protected void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj == null)
            {
                gameObject.SetActive(false);
            }
            else
            {
                player = obj.transform;
            }
        }
    }

    protected void Start()
    {
        agent.SetDestination(player.position);
    }
}
