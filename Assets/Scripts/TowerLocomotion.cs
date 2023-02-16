using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLocomotion : MonoBehaviour
{
    [Header("Tower Motion")]
    [SerializeField] float baseAscendSpeed = 1f;
    [SerializeField] float baseDropSpeed = 1f;
    [SerializeField] float regularDropMultiplier = 1f;
    [SerializeField] float armouredDropMultiplier = 1f;
    [SerializeField] float beefyDropMultiplier = 1.5f;
    [SerializeField] float bigBossDropMultiplier = 2f;
    [SerializeField] float towerHeight = 50f;

    public bool isAscending = false;

    private int[] enemyContactCount = new int[4] { 0, 0, 0, 0 };

    void Update()
    {
        TowerDescend();
        TowerAscend();
    }

    private void TowerDescend()
    {
        if (transform.position.y >= -towerHeight - 1f)
        {
            float combinedDropMultiplier = enemyContactCount[0] * regularDropMultiplier + enemyContactCount[1] * armouredDropMultiplier
                + enemyContactCount[2] * beefyDropMultiplier + enemyContactCount[3] * bigBossDropMultiplier;
            transform.position += Vector3.down * combinedDropMultiplier * baseDropSpeed * Time.deltaTime;
        }
    }

    private void TowerAscend()
    {
        if (isAscending && transform.position.y < 0)
            transform.position += Vector3.up * baseAscendSpeed * Time.deltaTime;
    }

    public void AddEnemyContact(int type)
    {
        enemyContactCount[type]++;
    }

    public void AddEnemyContact(string type)
    {
        if (type == "RegularEnemy")
            AddEnemyContact(0);
        if (type == "ArmouredEnemy")
            AddEnemyContact(1);
        if (type == "BeefyEnemy")
            AddEnemyContact(2);
        if (type == "BigBossEnemy")
            AddEnemyContact(3);
    }

    public void RemoveEnemyContact(int type)
    {
        if (enemyContactCount[type] > 0)
        {
            enemyContactCount[type]--;
        } else {
             enemyContactCount[type] = 0;
        }
    }

    public void RemoveEnemyContact(string type)
    {
        if (type == "RegularEnemy")
            RemoveEnemyContact(0);
        if (type == "ArmouredEnemy")
            RemoveEnemyContact(1);
        if (type == "BeefyEnemy")
            RemoveEnemyContact(2);
        if (type == "BigBossEnemy")
            RemoveEnemyContact(3);
    }
}
