using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using TMPro;
using UnityEngine;

public class StandardGameMode : GameMode
{
    private BossSpawner enemySpawner;
    private RoomManager roomManager;
    private float difficultyIncreaseFrequency = 3.0f;
    private double difficultyIncreaseTimeStamp;
    private int level;
    [SerializeField] private TextMeshProUGUI timerUI;
    [SerializeField] private int winTime = 240;
    private void Start()
    {
        enemySpawner = GetComponent<BossSpawner>();
        roomManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<RoomManager>();
        difficultyIncreaseTimeStamp = gameStopWatch.Elapsed.TotalSeconds;
        StartCoroutine(StartGame());
        Debug.Log("ran");
        difficulty = 1;
    }

    public void CheckLose()
    {
        bool allIsDead = true;
        for (int i = 0; i < RoomManager.players.Count; i++)
        {
            if (!RoomManager.players[i].GetComponent<Respawn>().dead)
            {
                allIsDead = false;
            }
        }
        if (allIsDead)
        {
            enemySpawner = GetComponent<BossSpawner>();
            enemySpawner.DespawnEnemies();
            roomManager.Lose();
            gameStopWatch.Reset();
            Destroy(gameObject);
        }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(startGameSeconds);
        gameStopWatch.Start();
        
        SpawnEnemy();
    }
    private void Update()
    {
        if (difficultyIncreaseTimeStamp - gameStopWatch.Elapsed.TotalSeconds < 0.0)
        {
            difficulty++;
            difficultyIncreaseTimeStamp = gameStopWatch.Elapsed.TotalSeconds + difficultyIncreaseFrequency;
        }
        timerUI.text = $"Time survived: {(int)gameStopWatch.Elapsed.TotalSeconds} / {winTime}";
        if (gameStopWatch.Elapsed.TotalSeconds >= winTime)
        {
            Win();
        }
    }

    private void Win()
    {
        enemySpawner = GetComponent<BossSpawner>();
        enemySpawner.DespawnEnemies();
        roomManager.Win();
        gameStopWatch.Reset();
        Destroy(gameObject);
    }
    private void SpawnEnemy()
    {
        enemySpawner.Spawn();
    }
}
