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
    [SerializeField] float towerMaxHeight = 50f;
    [Header("UI")]
    [SerializeField] PlayerUI playerUI;

    public bool isAscending = false;
    private int[] enemyContactCount = new int[4] { 0, 0, 0, 0 };
    private bool isFrozen = false;
    private float currentFreezeTime = 0f;
    private float totalFreezeTime = 0f;

    void FixedUpdate()
    {
        if (!isFrozen)
        {
            TowerDescend();
            TowerAscend();
            playerUI.UpdateHeight((towerMaxHeight + transform.position.y) / towerMaxHeight);
        }
        else
        {
            currentFreezeTime += Time.deltaTime;
            if (currentFreezeTime >= totalFreezeTime)
            {
                isFrozen = false;
                currentFreezeTime = 0f;
                totalFreezeTime = 0f;
            }
        }
    }

    private void TowerDescend()
    {
        if (transform.position.y >= -towerMaxHeight - 1f)
        {
            Debug.Log("(" + enemyContactCount[0] + ", " + enemyContactCount[1] + ", " + enemyContactCount[2] + ", " + enemyContactCount[3] + ")");
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

    public void SetAscending(bool ascending)
    {
        isAscending = ascending;
    }

    public void AddEnemyContact(int type)
    {
        enemyContactCount[type]++;
    }

    public void AddEnemyContact(string type)
    {
        if (type.Contains("RegularEnemy"))
            AddEnemyContact(0);
        if (type.Contains("ArmouredEnemy"))
            AddEnemyContact(1);
        if (type.Contains("BeefyEnemy"))
            AddEnemyContact(2);
        if (type.Contains("BigBossEnemy"))
            AddEnemyContact(3);
    }

    public void RemoveEnemyContact(int type)
    {
        if (enemyContactCount[type] > 0)
        {
            enemyContactCount[type]--;
        }
        else
        {
            enemyContactCount[type] = 0;
        }
    }

    public void RemoveEnemyContact(string type)
    {
        if (type.Contains("RegularEnemy"))
            RemoveEnemyContact(0);
        if (type.Contains("ArmouredEnemy"))
            RemoveEnemyContact(1);
        if (type.Contains("BeefyEnemy"))
            RemoveEnemyContact(2);
        if (type.Contains("BigBossEnemy"))
            RemoveEnemyContact(3);
    }

    public void FreezeTower(float seconds)
    {
        isFrozen = true;
        currentFreezeTime = 0f;
        totalFreezeTime = seconds;
    }
}
