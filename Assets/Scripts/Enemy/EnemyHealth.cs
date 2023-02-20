using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int health = 1;

    private TowerLocomotion towerLocomotion;
    private GameController gameController;
    private PlayerMana playerMana;

    void Awake()
    {
        towerLocomotion = GameObject.FindWithTag("Tower").GetComponent<TowerLocomotion>();
        gameController = GetComponentInParent<GameController>();
        playerMana = GameObject.FindWithTag("Player").GetComponent<PlayerMana>();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            towerLocomotion.RemoveEnemyContact(gameObject.name);
            gameController.RemoveEnemy(gameObject);
            playerMana.UpdateMana(1);
            Destroy(gameObject);
        }
    }
}
