using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    private Vector3 targetLocation;
    private TowerLocomotion towerLocomotion;
    private NavMeshAgent navMeshAgent;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        towerLocomotion = GameObject.FindWithTag("Tower").GetComponent<TowerLocomotion>();
    }

    void Start()
    {
        Ray ray = new Ray(transform.position, towerLocomotion.transform.position - transform.position);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, 1000, layerMask);
        targetLocation = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
        Debug.Log("Initial target: " + targetLocation);
    }

    void Update()
    {
        SetNavMeshDestination();
        Debug.DrawRay(transform.position, towerLocomotion.transform.position - transform.position);
    }

    private void SetNavMeshDestination()
    {
        if (navMeshAgent.enabled)
        {
            navMeshAgent.SetDestination(targetLocation);
            Debug.Log("Current destination: " + navMeshAgent.destination);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Freeze enemy in position when touching the tower
        if (other.gameObject.layer == 6 && navMeshAgent.enabled)
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
