using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject regularEnemy;
    [SerializeField] GameObject beefyEnemy;
    [SerializeField] GameObject armouredEnemy;
    [SerializeField] GameObject bigBossEnemy;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI gameOverSubtext;
    [SerializeField] string[] gameOverSubtextOptions;

    [SerializeField] int level = 1;
    private bool updatingLevel = false;
    private List<GameObject> enemies = new List<GameObject>();
    private CursorLockMode lockMode;
    private bool gameOverTriggered = false;

    private int totalEnemyMax = 0;
    private int enemiesOnScreenMax = 0;
    private int regularEnemyMax = 0;
    private int beefyEnemyMax = 0;
    private int armouredEnemyMax = 0;
    private int bigBossEnemyMax = 0;

    private int totalEnemyCount = 0;
    private int regularEnemyCount = 0;
    private int beefyEnemyCount = 0;
    private int armouredEnemyCount = 0;
    private int bigBossEnemyCount = 0;


    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start()
    {
        totalEnemyMax = 5 * level;
        enemiesOnScreenMax = level + 1;
        regularEnemyMax = 5;
        levelText.text = "Level: 1";
        gameOverText.text = "";
        gameOverSubtext.text = "";
    }

    void Update()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        while (enemies.Count < enemiesOnScreenMax && totalEnemyCount < totalEnemyMax)
        {
            //Generate random spawn location
            Vector2 randomPoint = Random.insideUnitCircle.normalized * 220;

            //Calculate which enemy type to generate
            if (enemies.Count < totalEnemyMax)
            {
                int enemyIdx = Random.Range(0, 4);
                switch (enemyIdx)
                {
                    case 0:
                        if (regularEnemyCount < regularEnemyMax)
                        {
                            enemies.Add(Instantiate(regularEnemy, new Vector3(randomPoint.x, 0, randomPoint.y), Quaternion.identity));
                            regularEnemyCount++;
                            totalEnemyCount++;
                        }
                        break;

                    case 1:
                        if (beefyEnemyCount < beefyEnemyMax)
                        {
                            enemies.Add(Instantiate(beefyEnemy, new Vector3(randomPoint.x, 0, randomPoint.y), Quaternion.identity));
                            beefyEnemyCount++;
                            totalEnemyCount++;
                        }
                        break;

                    case 2:
                        if (armouredEnemyCount < armouredEnemyMax)
                        {
                            enemies.Add(Instantiate(armouredEnemy, new Vector3(randomPoint.x, 0, randomPoint.y), Quaternion.identity));
                            armouredEnemyCount++;
                            totalEnemyCount++;
                        }
                        break;

                    case 3:
                        if (bigBossEnemyCount < bigBossEnemyMax)
                        {
                            enemies.Add(Instantiate(bigBossEnemy, new Vector3(randomPoint.x, 0, randomPoint.y), Quaternion.identity));
                            bigBossEnemyCount++;
                            totalEnemyCount++;
                        }
                        break;
                }
            }
        }

        //Check if level is complete
        if (enemies.Count == 0 && totalEnemyCount >= totalEnemyMax)
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

        //Clear current enemies
        enemies.Clear();
        totalEnemyCount = 0;
        regularEnemyCount = 0;
        beefyEnemyCount = 0;
        armouredEnemyCount = 0;
        bigBossEnemyCount = 0;

        //Setup enemies for next level
        level++;
        totalEnemyMax = 5 * level;
        enemiesOnScreenMax = level + 1;
        beefyEnemyMax = level - 1;
        if (level >= 5)
            armouredEnemyMax = level - 4;
        if (level >= 10)
            bigBossEnemyMax = level - 9;
        regularEnemyMax = totalEnemyMax - beefyEnemyMax - armouredEnemyMax - bigBossEnemyMax;

        //Change level text
        levelText.color = Color.white;
        levelText.text = "Level: " + level;
        updatingLevel = false;
    }

    public void RemoveEnemy(GameObject enemyToRemove)
    {
        Debug.Log("Current enemy count: " + enemies.Count);
        enemies.Remove(enemyToRemove);
    }

    public void Cheat()
    {
        int total = enemies.Count;
        for (int i = 0; i < total; i++)
        {
            enemies[0].GetComponent<EnemyHealth>().TakeDamage(1000);
        }
    }

    public void GameOver()
    {
        if (!gameOverTriggered)
        {
            StartCoroutine(GameOverLogic());
            gameOverTriggered = true;
        }
    }

    private IEnumerator GameOverLogic()
    {
        gameOverText.color = Color.red;
        gameOverText.fontSize = 100;
        gameOverText.fontStyle = FontStyles.Bold;
        gameOverText.text = "GAME OVER:";
        gameOverSubtext.text = gameOverSubtextOptions[Random.Range(0, gameOverSubtextOptions.Length)];
        yield return new WaitForSeconds(3f);
        gameOverText.color = Color.magenta;
        gameOverText.fontSize = 60;
        gameOverText.fontStyle = FontStyles.Normal;
        for (int i = 5; i > 0; i--)
        {
            gameOverText.text = "New game in " + i;
            yield return new WaitForSeconds(1f);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
