using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossBehavior : MonoBehaviour
{
    private Vector2 topLeftCorner = new(0, 0);
    private Vector2 bottomRightCorner = new (20f, -8f);
    private Vector2 targetLocation = new (10, -4);
    
    private float speed = 2f;
    
    private double moveTime = 2f;
    private double waitTime = 1f;
    private double newTimeStamp;
    
    private Multiplayer multiplayer;
    
    [SerializeField] private ProjectileCoordinator projectileCoordinator;

    private void Start()
    {
        Random.InitState(500);
        newTimeStamp = moveTime + waitTime + GameMode.gameStopWatch.Elapsed.TotalSeconds;
        targetLocation = GetRandomLocation();
    }

    void Update()
    {
        MoveTowardsLocation(targetLocation);
        UpdateTargetLocation();
    }

    private void UpdateTargetLocation()
    {
        if ((int) newTimeStamp - GameMode.gameStopWatch.Elapsed.TotalSeconds <= 0)
        {
            newTimeStamp = moveTime + waitTime + GameMode.gameStopWatch.Elapsed.TotalSeconds;
            
            targetLocation = GetRandomLocation();
            ProjectileData projectileData;
            {
                projectileData.origin = transform.position;
                projectileData.velocity = Vector3.right * 1;
                projectileData.projectileType = ProjectileType.Linear;
                projectileData.lifeTime = 9;
            }
            ShootRandomShot(projectileData);
        }
    }

    private void ShootRandomShot(ProjectileData projectileData)
    {
        int weapon = Random.Range(0, 2);
        switch (weapon)
        {
            case 0 :
            projectileCoordinator.SquareShot(projectileData, 2 * GameMode.difficulty);
                break;
            case 1 :
            projectileCoordinator.SphericalShot(projectileData, 3 * GameMode.difficulty);
            break;
        }
    }
    private void MoveTowardsLocation(Vector3 location)
    {
        float t = Mathf.Clamp( ((float)(GameMode.gameStopWatch.Elapsed.TotalSeconds - newTimeStamp + waitTime)), 0f, 1f);
        transform.position = Vector3.Lerp(transform.position, location, EaseInOutCirc(t));
    }

    private Vector2 GetRandomLocation()
    {
        return new Vector2(Random.Range(topLeftCorner.x, bottomRightCorner.x),
            Random.Range(bottomRightCorner.y, topLeftCorner.y));
    }
    float EaseInOutCirc(float x) 
    {
        return x < 0.5 ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2
            : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2;
    }
}
