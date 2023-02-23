using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] LayerMask destroyWhenTouchingLayer;

    private Rigidbody rb;
    private Transform updatedTransform;
    private bool projectileReleased = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void Start()
    {
    }

    void Update()
    {
        if (!projectileReleased && updatedTransform != null)
        {
            this.transform.position = updatedTransform.position;
        }
    }

    public void SetTransform(Transform transform)
    {
        updatedTransform = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        //Check if projectile hit layer mask
        if (projectileReleased && (destroyWhenTouchingLayer.value & (1 << other.gameObject.layer)) > 0)
            Destroy(gameObject);
    }

    public void ReleaseProjectile()
    {
        rb.useGravity = true;
        projectileReleased = true;
    }
}
