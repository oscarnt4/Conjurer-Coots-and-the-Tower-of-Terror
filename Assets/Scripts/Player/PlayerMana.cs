using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    private PlayerUI playerUI;

    private int currentMana = 0;
    private int maxMana = 100;

    void Awake()
    {
        playerUI = GetComponent<PlayerUI>();
    }

    void Start()
    {
        playerUI.UpdateMana(currentMana);
    }

    public bool UpdateMana(int amountToAdd)
    {
        int newManaAmount = currentMana + amountToAdd;
        if (newManaAmount < 0)
        {
            return false;
        }
        else if (newManaAmount > maxMana)
        {
            currentMana = maxMana;
        }
        else
        {
            currentMana = newManaAmount;
        }
        playerUI.UpdateMana(currentMana);
        return true;
    }
}
