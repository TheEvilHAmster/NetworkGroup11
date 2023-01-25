using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class StandardGameMode : GameMode
{
    private BossSpawner enemySpawner;
    private Spawner spawner;
    
    private int difficultyIncreaseFrequency = 5;
    private double difficultyIncreaseTimeStamp;
    private int level;
    
    private void Start()
    {
        enemySpawner = GetComponent<BossSpawner>();
        difficultyIncreaseTimeStamp = gameStopWatch.Elapsed.TotalSeconds;
        StartCoroutine(StartGame());
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
            GameMode.difficulty++;
            Debug.Log(difficulty);
            difficultyIncreaseTimeStamp = gameStopWatch.Elapsed.TotalSeconds + difficultyIncreaseFrequency;
        }
    }
    
    private void SpawnEnemy()
    {
        enemySpawner.Spawn();
    }
}
