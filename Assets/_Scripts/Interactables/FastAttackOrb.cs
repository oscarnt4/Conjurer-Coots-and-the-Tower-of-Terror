using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastAttackOrb : Interactable
{
    [SerializeField] PlayerAttack playerAttack;
    [SerializeField] float attackChargeTime = 2f;
    [SerializeField] PlayerMana playerMana;
    [SerializeField] int manaCost = 10;
    [SerializeField] PlayerUI playerUI;

    private Material orbMaterial;
    private Color originalColour;
    private bool isCharging = false;
    private float currentChargeTime = 0f;

    void Awake()
    {
        orbMaterial = GetComponent<Renderer>().material;
    }
    void Start()
    {
        originalColour = orbMaterial.color;
    }

    void Update()
    {
        if (isCharging)
        {
            currentChargeTime += Time.deltaTime;
        }
        if (currentChargeTime >= attackChargeTime)
        {
            orbMaterial.color = Color.red;
        }
    }

    protected override void Interact()
    {
        if (!isCharging)
        {
            if (playerMana.UpdateMana(-manaCost))
            {
                playerMana.UpdateMana(manaCost);
                orbMaterial.EnableKeyword("_EMISSION");
                if (!isCharging)
                {
                    isCharging = true;
                    currentChargeTime = 0f;
                }
            }
            else
            {
                playerUI.UpdateText("NOT ENOUGH MANA");
            }
        }
        if (isCharging && currentChargeTime < attackChargeTime)
        {
            playerUI.UpdateText("Charging...");
        }
    }

    protected override void ReleaseInteract()
    {
        if (isCharging)
        {
            if (currentChargeTime >= attackChargeTime)
            {
                playerMana.UpdateMana(-manaCost);
                playerAttack.SwitchState(1);
            }
        }

        currentChargeTime = 0f;
        orbMaterial.DisableKeyword("_EMISSION");
        orbMaterial.color = originalColour;
        isCharging = false;
    }
}
