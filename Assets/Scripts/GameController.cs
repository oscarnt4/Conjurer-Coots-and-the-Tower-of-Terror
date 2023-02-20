using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject regularEnemy;
    [SerializeField] GameObject beefyEnemy;
    [SerializeField] GameObject armouredEnemy;
    [SerializeField] GameObject bigBossEnemy;
    [SerializeField] TextMeshProUGUI levelText;

    private Transform[] spawnLocations;
    private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] int level = 1;
    private int enemyLevelTotal;
    private int enemiesAllowedOnScreen;
    private int currentEnemyLevelTotal = 0;
    private bool updatingLevel = false;
    private float spawnRadiusMax = 20f;

    void Awake()
    {
        spawnLocations = GetComponentsInChildren<Transform>();
    }
    void Start()
    {
        enemyLevelTotal = 5 * level;
        enemiesAllowedOnScreen = level + 1;
        foreach (Transform spawnLocation in spawnLocations)
        {
            if (spawnLocation.tag != "Spawn")
            {
                List<Transform> spawnLocationsList = new List<Transform>(spawnLocations);
                spawnLocationsList.Remove(spawnLocation);
                spawnLocations = spawnLocationsList.ToArray();
            }
        }
        levelText.text = "Level: 1";
    }

    void Update()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        while (enemies.Count < enemiesAllowedOnScreen && currentEnemyLevelTotal < enemyLevelTotal)
        {
            //Generate random spawn location
            Transform spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Length - 1)];
            Vector2 randomPoint = Random.insideUnitCircle * Random.Range(0, spawnRadiusMax);
            spawnLocation.position = spawnLocation.position + new Vector3(randomPoint.x, 0,randomPoint.y);

            //Calculate which enemy type to generate
            if (((currentEnemyLevelTotal - 1) / 5) % 1 == 0 && currentEnemyLevelTotal > 5)
            {
                enemies.Add(Instantiate(beefyEnemy, spawnLocation));
            }
            else if (((currentEnemyLevelTotal - 2) / 5) % 1 == 0 && currentEnemyLevelTotal > 20)
            {
                enemies.Add(Instantiate(armouredEnemy, spawnLocation));
            }
            else
            {
                enemies.Add(Instantiate(regularEnemy, spawnLocation));
            }
            currentEnemyLevelTotal++;
            Debug.Log("Current enemy level total " + currentEnemyLevelTotal);
        }
        //Check if level is complete
        if (enemies.Count == 0 && currentEnemyLevelTotal >= enemyLevelTotal)
        {
            if (!updatingLevel)
                StartCoroutine(IncreaseLevel());
        }
    }

    private IEnumerator IncreaseLevel()
    {
        updatingLevel = true;
        //Generate end of level text
        levelText.color = Color.green;
        levelText.text = "LEVEL " + level + " COMPLETE";
        yield return new WaitForSeconds(2f);
        levelText.color = Color.red;
        for (int i = 3; i > 0; i--)
        {
            levelText.text = "Level " + (level + 1) + " beginning in " + i;
            yield return new WaitForSeconds(1f);
        }
        //Setup for next level
        enemies.Clear();
        currentEnemyLevelTotal = 0;
        level++;
        enemyLevelTotal = 5 * level;
        enemiesAllowedOnScreen = level + 1;
        levelText.color = Color.white;
        levelText.text = "Level: " + level;
        updatingLevel = false;
    }

    public void RemoveEnemy(GameObject enemyToRemove)
    {
        enemies.Remove(enemyToRemove);
    }
}
