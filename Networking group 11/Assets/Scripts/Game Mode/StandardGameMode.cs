using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class StandardGameMode : GameMode
{
    private BossSpawner enemySpawner;
    private int level;
    private Spawner spawner;
    private void Start()
    {
        enemySpawner = GetComponent<BossSpawner>();
        SpawnEnemy();
    }

    private void Update()
    {
        timePassedSinceStart += Time.deltaTime;
    }

    private void SpawnEnemy()
    {
        enemySpawner.Spawn();
    }
}
