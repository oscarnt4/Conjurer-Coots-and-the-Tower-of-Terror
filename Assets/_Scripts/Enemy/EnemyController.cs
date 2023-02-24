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
    private Animator animator;
    private AudioSource meow;
    private bool isMeowing = false;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        towerLocomotion = GameObject.FindWithTag("Tower").GetComponent<TowerLocomotion>();
        animator = GetComponentInChildren<Animator>();
        meow = GetComponent<AudioSource>();
    }

    void Start()
    {
        Ray ray = new Ray(transform.position, towerLocomotion.transform.position - transform.position);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, 1000, layerMask);
        targetLocation = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
        Debug.Log("Initial target: " + targetLocation);
        meow.pitch += Random.Range(-0.25f,0.25f);
    }

    void Update()
    {
        SetNavMeshDestination();
        Debug.DrawRay(transform.position, towerLocomotion.transform.position - transform.position);
        animator.SetBool("isRunning", navMeshAgent.velocity.magnitude > 0);
        if (!isMeowing)
        {
            StartCoroutine(PlayAudio());
        }
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

    private IEnumerator PlayAudio()
    {
        isMeowing = true;
        yield return new WaitForSeconds(Random.Range(0f,10f));
        Debug.Log("Meow pitch: " + meow.pitch);
        meow.Play();
        yield return new WaitForSeconds(meow.clip.length);
        isMeowing = false;
    }
}
