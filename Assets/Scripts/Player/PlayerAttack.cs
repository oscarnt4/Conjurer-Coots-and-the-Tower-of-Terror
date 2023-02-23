using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float chargeTime = 1f;
    [SerializeField] float rapidFireDuration = 7f;
    [SerializeField] Transform projectileSpawnLocation;
    [SerializeField] GameObject baseProjectile;
    [SerializeField] GameObject bombProjectile;

    private bool isActive = true;
    private int stateIdx = 0;
    private bool chargingShot = false;
    private bool shotCharged = false;
    private float currentChargeTime = 0f;
    private float currentRapidFireDuration = 0f;
    private GameObject projectile;
    private InputManager inputManager;

    void Awake()
    {
        inputManager = GetComponent<InputManager>();
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            Attack();
        }
    }

    public void SetActive(bool activate)
    {
        isActive = activate;
    }

    private void Attack()
    {
        //Check attack type
        switch (stateIdx)
        {
            case 0:
                BaseAttack();
                break;

            case 1:
                RapidFire();
                break;

            case 2:
                BombAttack();
                break;

            default:
                BaseAttack();
                break;
        }
    }

    public void SwitchState(int stateIdx)
    {
        this.stateIdx = stateIdx;
    }

    private void BaseAttack()
    {
        //Instantiate projectile prefab
        if (inputManager.defaultActions.Shoot.IsPressed() && !chargingShot)
        {
            projectile = Instantiate(baseProjectile, projectileSpawnLocation.position, Quaternion.identity);
            projectile.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            projectile.GetComponent<Projectile>().SetTransform(projectileSpawnLocation);

            chargingShot = true;
            currentChargeTime = 0f;
        }
        //Update charging status of projectile
        if (chargingShot)
        {
            currentChargeTime += Time.deltaTime;

            if (currentChargeTime >= chargeTime && !shotCharged)
            {
                projectile.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                shotCharged = true;
            }
        }
        //Release projectile
        if (!inputManager.defaultActions.Shoot.IsPressed())
        {
            //Check charge status of released projectile
            if (shotCharged)
            {
                if (projectile != null)
                {
                    projectile.GetComponent<Projectile>().ReleaseProjectile();
                }
                shotCharged = false;
            }
            else
            {
                if (projectile != null && chargingShot)
                {
                    Destroy(projectile);
                }
            }
            chargingShot = false;
            currentChargeTime = 0f;
        }
    }

    private void RapidFire()
    {
        //Update rapid fire duration
        currentRapidFireDuration += Time.deltaTime;
        //Instantiate projectil
        if (inputManager.defaultActions.Shoot.IsPressed() && !shotCharged)
        {
            projectile = Instantiate(baseProjectile, projectileSpawnLocation.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().SetTransform(projectileSpawnLocation);
            shotCharged = true;
        }
        //Release projectile
        if (!inputManager.defaultActions.Shoot.IsPressed() && shotCharged)
        {
            if (projectile != null)
                projectile.GetComponent<Projectile>().ReleaseProjectile();
            shotCharged = false;
        }
        //Check rapid fire duration and return to base attack when finished
        if (currentRapidFireDuration >= rapidFireDuration)
        {
            if (projectile != null)
                projectile.GetComponent<Projectile>().ReleaseProjectile();
            stateIdx = 0;
            currentRapidFireDuration = 0f;
        }
    }

    private void BombAttack()
    {
        //Instantiate projectile
        if (inputManager.defaultActions.Shoot.IsPressed() && !shotCharged)
        {
            projectile = Instantiate(bombProjectile, projectileSpawnLocation.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().SetTransform(projectileSpawnLocation);
            shotCharged = true;
        }
        //Release projectile
        if (!inputManager.defaultActions.Shoot.IsPressed() && shotCharged)
        {
            if (projectile != null)
                projectile.GetComponent<Projectile>().ReleaseProjectile();
            stateIdx = 0;
            shotCharged = false;
        }
    }
}
