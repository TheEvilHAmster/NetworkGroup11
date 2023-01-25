using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossBehavior : MonoBehaviour
{
    private Vector2 topLeftCorner = new(0.7f, -0.8f);
    private Vector2 bottomRightCorner = new (19.25f, -8.1f);
    private Vector2 targetLocation = new (10, -4);
    
    private float speed = 2f;
    
    private double moveTime = 2f;
    private double waitTime = 1f;
    private double newTimeStamp;
    
    private Multiplayer multiplayer;
    
    [SerializeField] private ProjectileCoordinator projectileCoordinator;

    private void Start()
    {
        Random.InitState(120);
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
                projectileData.velocity = Vector2.up * 1;
                projectileData.projectileType = ProjectileType.Linear;
                projectileData.lifeTime = 9;
                projectileData.target = null;
            }
            ShootRandomShot(projectileData);
        }
    }

    private void ShootRandomShot(ProjectileData projectileData)
    {
        int weaponsUnlocked = Mathf.Clamp( GameMode.difficulty / 3, 0, 5);
        int weapon = Random.Range(0, weaponsUnlocked);
        switch (weapon)
        {
            case 0 :
                projectileCoordinator.SphericalShot(projectileData, (int) (1f * GameMode.difficulty));
                break;
            case 1 :
                projectileCoordinator.RapidSphericalShot(projectileData, (int) (1.5f * GameMode.difficulty));
                break;
            case 2 :
                projectileData.projectileType = ProjectileType.Homing;
                
                projectileCoordinator.RapidFireShot(projectileData, (int) (0.35f * GameMode.difficulty), Vector2.up);
                break;
            case 3 :
                projectileData.projectileType = ProjectileType.Homing;
                projectileCoordinator.RapidSphericalShot(projectileData, (int) (0.35f * GameMode.difficulty));
                break;
            case 4 :
                projectileData.projectileType = ProjectileType.Homing;
                
                projectileCoordinator.RapidFireShot(projectileData, (int) (0.45f * GameMode.difficulty), Vector2.up);
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
