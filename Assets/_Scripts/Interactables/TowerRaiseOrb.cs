using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRaiseOrb : Interactable
{
    private TowerLocomotion towerLocomotion;
    private Material orbMaterial;
    void Awake()
    {
        towerLocomotion = GetComponentInParent<TowerLocomotion>();
        orbMaterial = GetComponent<Renderer>().material;
    }

    protected override void Interact()
    {
        towerLocomotion.SetAscending(true);
        orbMaterial.EnableKeyword("_EMISSION");
    }

    protected override void ReleaseInteract()
    {
        towerLocomotion.SetAscending(false);
        orbMaterial.DisableKeyword("_EMISSION");
    }
}
