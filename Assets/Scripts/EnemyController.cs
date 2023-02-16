using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform playerLocation;

    private TowerLocomotion towerLocomotion;
    private NavMeshAgent navMeshAgent;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        towerLocomotion = GameObject.FindWithTag("Tower").GetComponent<TowerLocomotion>();
    }

    void Update()
    {
        SetNavMeshDestination();
    }

    private void SetNavMeshDestination()
    {
        if (navMeshAgent.enabled)
        {
            Vector3 destination = new Vector3(playerLocation.position.x, 0, playerLocation.position.z);
            navMeshAgent.SetDestination(destination);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Freeze enemy in position when touching the tower
        if (other.gameObject.layer == 6 && navMeshAgent.enabled == true)
        {
            navMeshAgent.enabled = false;
            towerLocomotion.AddEnemyContact(gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Enable navmesh when tower has gone
        if (other.gameObject.layer == 6)
        {
            navMeshAgent.enabled = true;
            towerLocomotion.RemoveEnemyContact(gameObject.name);
        }
    }
}
