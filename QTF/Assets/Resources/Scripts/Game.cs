using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

[Serializable]
public class Game : MonoBehaviour
{
    //prefabs
    public GameObject playerObjectPrefab;
    public GameObject enemySpawnPointPrefab;
    public GameObject enemyPrefab;
    public GameObject enemyHeavyPrefab;
    public GameObject enemyInverterPrefab;
    public GameObject enemyCivilianPrefab;
    public GameObject bonusPrefab;
    public GameObject healthContainerPrefab;
    public GameObject fullHealthContainerPrefab;
    

    //UI
    public GameObject gameOverScreen;
    public GameObject gameHUDScreen;
    public GameObject healthPanel;
    public SpriteRenderer backgroundSprite;
    public Text scoreText;
    public Text invertedText;

    //Colours
    public List<Color> backgroundColoursAvailable = new List<Color>();

    //Roots
    public GameObject enemiesRoot;
    public GameObject bulletsRoot;
    public GameObject bonusesRoot;

    private Vector2 screenResolution;
    private GameObject playerObject = null;
    private Player playerScript;
    private List<EnemySpawnPoint> enemySpawnPoint;
    private List<GameObject> healthContainersList;
    private float enemySpeed;
    private float enemyCount;
    private float spawnDelay;
    private float spawnDelayRemaining;
    private float reversedControlTimeRemaining;
    private float backgroundChangeTimer = 10.0f;
    private float backgroundChangeTimeRemaining = 10.0f;
    private Random random;
    private bool wasPressed = false;
    private int enemyKilledCount;
    private int lastDifficultyChangeKillCount;

    public static Game instance
    {
        get;
        private set;
    }

    private int playerScore;

    public int score
    {
        set
        {
            playerScore = value;
            UpdateScoreBox();
        }

        get
        {
            return playerScore;
        }
    }

    private void UpdateScoreBox()
    {
        scoreText.text = "Score : " + score.ToString();
    }

    public void RewardPlayer(int points)
    {
        score = playerScore + points;
    }

    public void RewardPlayer(Bonus bonus)
    {
        Player.instance.AddBonus(bonus);
    }


    // Use this for initialization
    void Start ()
    {
        if (instance != null && instance.gameObject != gameObject)
        {
            Destroy(gameObject);
            return;
        }
        else instance = this;


        screenResolution = new Vector2(480, 800);
        enemySpawnPoint = new List<EnemySpawnPoint>();
        
        GameObject centerSpawnPoint = Instantiate(enemySpawnPointPrefab);
        Vector3 centerPosition = new Vector3(screenResolution.x / 2, 700, 1);
        centerSpawnPoint.transform.position = centerPosition;
        centerSpawnPoint.transform.SetParent(transform.parent);

        Inverter.SetBulletRoot(bulletsRoot);

        for (int i = 2; i >= 1; i--)
        {
            GameObject newSpawnPoint = Instantiate(enemySpawnPointPrefab);
            Vector3 newSpawnPointPosition = new Vector3(centerPosition.x - i * (screenResolution.x / 5), centerPosition.y, 1);
            newSpawnPoint.transform.position = newSpawnPointPosition;
            newSpawnPoint.transform.SetParent(transform.parent);
            enemySpawnPoint.Add(newSpawnPoint.GetComponent<EnemySpawnPoint>());
        }

        enemySpawnPoint.Add(centerSpawnPoint.GetComponent<EnemySpawnPoint>());

        for (int i = 1; i <= 2; i++)
        {
            GameObject newSpawnPoint = Instantiate(enemySpawnPointPrefab);
            Vector3 newSpawnPointPosition = new Vector3(centerPosition.x + i * (screenResolution.x / 5), centerPosition.y, 1);
            newSpawnPoint.transform.position = newSpawnPointPosition;
            newSpawnPoint.transform.SetParent(transform.parent);
            enemySpawnPoint.Add(newSpawnPoint.GetComponent<EnemySpawnPoint>());
        }

        random = new Random();
    }
	
	// Update is called once per frame
	void Update ()
    {
        ParseInput();
        GameLogic();
    }

    private void ParseInput()
    {
        if (wasPressed == false)
        {
            foreach (var touch in Input.touches)
            {
                wasPressed = true;
                Vector2 position = touch.position;
                if (position.x <= screenResolution.x / 2 && position.y > 150) LeftDirectionTouch();
                else if (position.x >= screenResolution.x / 2 && position.y > 150) RightDirectionTouch();
                else ShootTouch();
            }
        }

        if (Input.touchCount <= 0) wasPressed = false;
    }

    private void GameLogic()
    {
        SpawnLogic();
        DifficultyLogic();

        if (reversedControlTimeRemaining > 0)
        {
            reversedControlTimeRemaining -= Time.deltaTime;
            invertedText.text = "Inverted : " + reversedControlTimeRemaining.ToString().Substring(0, 3);
        }
        else invertedText.text = "";

        if (Player.instance.currentHP <= 0)
        {
            Debug.Log("ded");
            StopGame();
        }

        backgroundChangeTimeRemaining -= Time.deltaTime;
        if(backgroundChangeTimeRemaining <= 0)
        {
            backgroundChangeTimeRemaining = backgroundChangeTimer;
            int randomIndex = random.Next(0, backgroundColoursAvailable.Count);
            Sprite sprite = backgroundSprite.sprite;
            backgroundSprite.color = backgroundColoursAvailable[randomIndex];
            backgroundSprite.sprite = sprite;

            Debug.Log("Changing colour to " + backgroundSprite.color);
        }

    }

    private void StopGame()
    {
        DestroyAllObjects();
        gameOverScreen.SetActive(true);
        enabled = false;
        gameHUDScreen.SetActive(false);
        Leaderboard.instance.AddScore(score);
        gameOverScreen.transform.parent.Find("LeaderboardsScreen").GetComponent<LeaderboardsScreen>().Refresh();
    }


    public void ChangeHealthDisplayed(int current, int max)
    {
        for (int i = 0; i < healthContainersList.Count; i++) Destroy(healthContainersList[i].gameObject);
        healthContainersList.Clear();

        for(int i = 0; i < current; i++)
        {
            GameObject fullContainer = Instantiate(fullHealthContainerPrefab);
            fullContainer.transform.SetParent(healthPanel.transform);
            healthContainersList.Add(fullContainer);
        }

        for (int i = 0; i < max - current; i++)
        {
            GameObject container = Instantiate(healthContainerPrefab);
            container.transform.SetParent(healthPanel.transform);
            healthContainersList.Add(container);
        }
    }

    private int GetRandomFreeSpawnPoint()
    {
        List<EnemySpawnPoint> freeSpawnPoints = new List<EnemySpawnPoint>();
        foreach(var spawnPoint in enemySpawnPoint)
        {
            if (!spawnPoint.isLocked) freeSpawnPoints.Add(spawnPoint);
        }

        int randomIndex = random.Next(0, freeSpawnPoints.Count);
        EnemySpawnPoint randomizedSpawnPoint = freeSpawnPoints[randomIndex];

        return enemySpawnPoint.IndexOf(randomizedSpawnPoint);
       
    }

    private void SpawnLogic()
    {
        spawnDelayRemaining -= Time.deltaTime;
        if (spawnDelayRemaining <= 0)
        {
            float diceRoll = (float) random.NextDouble();
            if (diceRoll > enemyCount) SpawnRandomBonus();
            else SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int randomSpawn = GetRandomFreeSpawnPoint();

        GameObject newEnemy = Instantiate(GetRandomEnemyPrefab());
        EnemySpawnPoint spawnPoint = enemySpawnPoint[randomSpawn];
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();

        //if it is an inverter we need to setup bullet root

        newEnemy.transform.SetParent(enemiesRoot.transform);
        enemyScript.SetSpeed(enemySpeed);
        spawnPoint.Spawn(newEnemy);
        spawnDelayRemaining = spawnDelay;
        enemyKilledCount++;
    }

    private GameObject GetRandomEnemyPrefab()
    {
        float diceRoll = (float)random.NextDouble();
        if (diceRoll <= 0.5f) return enemyPrefab;
        else if (diceRoll <= 0.6f) return enemyCivilianPrefab;
        else if (diceRoll <= 0.75f) return enemyInverterPrefab;
        else return enemyHeavyPrefab;
    }

    public void ReverseControl()
    {
        reversedControlTimeRemaining = 5.0f;
    }

    private void SpawnRandomBonus()
    {
        GameObject bonus = Instantiate(bonusPrefab);
        bonus.transform.SetParent(bonusesRoot.transform);

        Bonus bonusScript = bonus.GetComponent<Bonus>();
        bonusScript.SetSpeed(enemySpeed);
        bonusScript.CreateBonus((EBonusType)random.Next(0, (int)EBonusType.COUNT));

        int randomSpawn = GetRandomFreeSpawnPoint();
        enemySpawnPoint[randomSpawn].Spawn(bonus);
        spawnDelayRemaining = spawnDelay;
    }

    private void DifficultyLogic()
    {
        if(enemyKilledCount % 20 == 0 && lastDifficultyChangeKillCount != enemyKilledCount)
        {
            lastDifficultyChangeKillCount = enemyKilledCount;
            enemySpeed *= 1.1f;
            if(enemyCount < 0.95f) enemyCount *= 1.01f;
            spawnDelay *= 0.95f;

            Debug.Log("Difficulty changed. Enemy speed is now " + enemySpeed + ", enemy percentage is now " + enemyCount + ", spawn timer is now " + spawnDelay);
        }
    }

    public void NewGame()
    {
        Player.DeleteInstance();
        playerObject = Instantiate(playerObjectPrefab);
        playerObject.transform.SetParent(transform.parent);
        playerScript = playerObject.GetComponent<Player>();

        if(healthContainersList == null)
        {
            healthContainersList = new List<GameObject>();
        }

        ChangeHealthDisplayed(3, 3);

        foreach(var container in healthContainersList)
        {
            container.transform.SetParent(healthPanel.transform);
        }


        score = 0;
        enemySpeed = 150;
        enemyCount = 0.85f;
        spawnDelay = 1.5f;
        spawnDelayRemaining = 0;
        enemyKilledCount = 0;
        reversedControlTimeRemaining = 0;
        lastDifficultyChangeKillCount = 0;
        backgroundChangeTimeRemaining = 0.00f;
        enabled = true;
        gameHUDScreen.SetActive(true);
    }

    private void DestroyAllObjects()
    {
        List<GameObject> objectsToDelete = new List<GameObject>();

        for (int i = 0; i < enemiesRoot.transform.childCount; i++)
            objectsToDelete.Add(enemiesRoot.transform.GetChild(i).gameObject);

        for (int i = 0; i < bulletsRoot.transform.childCount; i++)
            objectsToDelete.Add(bulletsRoot.transform.GetChild(i).gameObject);

        for (int i = 0; i < bonusesRoot.transform.childCount; i++)
            objectsToDelete.Add(bonusesRoot.transform.GetChild(i).gameObject);

        foreach (var obj in objectsToDelete)
        {
            Destroy(obj);
        }
        objectsToDelete.Clear();

        enemiesRoot.transform.DetachChildren();
        bonusesRoot.transform.DetachChildren();
        bulletsRoot.transform.DetachChildren();
    }

    public void LeftDirectionTouch()
    {
        if (reversedControlTimeRemaining <= 0) playerScript.MoveLeft();
        else playerScript.MoveRight();
    }

    public void RightDirectionTouch()
    {
        if (reversedControlTimeRemaining <= 0) playerScript.MoveRight();
        else playerScript.MoveLeft();
    }

    public void ShootTouch()
    {
        playerScript.Shoot(bulletsRoot);
    }

}
