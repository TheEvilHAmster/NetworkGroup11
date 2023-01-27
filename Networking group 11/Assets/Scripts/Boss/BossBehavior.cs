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
    private float reloadTime = 0;
    private Multiplayer multiplayer;
    
    [SerializeField] private ProjectileCoordinator projectileCoordinator;

    private void Start()
    {
        Random.InitState(120);
        newTimeStamp = moveTime + waitTime + GameMode.gameStopWatch.Elapsed.TotalSeconds + reloadTime;
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
            StartCoroutine(Shoot(Random.Range(0.5f, 1.2f)));
            newTimeStamp = moveTime + waitTime + GameMode.gameStopWatch.Elapsed.TotalSeconds;
        }
    }

    private IEnumerator Shoot(float delay)
    {
        yield return new WaitForSeconds(delay);
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
        newTimeStamp += reloadTime;
    }
    private void ShootRandomShot(ProjectileData projectileData)
    {
        int weaponsUnlocked = Mathf.Clamp( (int)(GameMode.difficulty / 3f), 0, 10);
        int weapon = Random.Range(0, weaponsUnlocked);
        switch (weapon)
        {
            case 0 :
                projectileCoordinator.SphericalShot(projectileData, (int) (0.7f * GameMode.difficulty));
                projectileData.lifeTime = 23f;
                reloadTime = 0.1f;
                break;
            case 1 :
                projectileData.lifeTime = 21f;
                projectileCoordinator.RapidSphericalShot(projectileData, (int) (1f * GameMode.difficulty), 1.25f);
                reloadTime = 0.1f;
                break;
            case 2 :
                projectileData.projectileType = ProjectileType.Homing;
                projectileData.lifeTime = 14f;
                projectileCoordinator.RapidFireShot(projectileData, (int) (0.65f * GameMode.difficulty), Vector2.up);
                reloadTime = 0.1f;
                break;
            case 3 :
                projectileCoordinator.RapidSphericalShot(projectileData, (int) (0.85f * GameMode.difficulty), 1.15f);
                projectileData.lifeTime = 21f;
                reloadTime = 0.6f;
                break;
            case 4 :
                reloadTime = 3.5f;
                projectileData.lifeTime = 21f;
                StartCoroutine(ChargeShotLinear(projectileData, 1.5f));
                break;
            case 5 :
                projectileData.lifeTime = 13;
                StartCoroutine(ChargeShotRapidRockets(projectileData, 1));
                reloadTime = 3f;
                break;
            case 6 :
                projectileData.lifeTime = 13.5f;
                projectileData.velocity = Vector2.up * 0.55f;
                StartCoroutine(ChargeShotRocketsSpherical(projectileData, 1.5f));
                reloadTime = 5f;
                break;
            case 7 :
                projectileData.lifeTime = 12f;
                StartCoroutine(ChargeShotRapidRockets(projectileData, 2));
                StartCoroutine(ChargeShotLinear(projectileData, 1.5f));
                reloadTime = 5f;
                break;
            
            case 8 :
                projectileData.lifeTime = 16f;
                StartCoroutine(RapidFireSpherical(projectileData, 1.5f, 3));
                reloadTime = 2f;
                break;
            case 9 :
                projectileData.lifeTime = 16f;
                projectileCoordinator.RapidSphericalShotGaps(projectileData, (int) (2f * GameMode.difficulty), 3.25f);
                reloadTime = 0.9f;
                break;
        }
    }

    private IEnumerator ChargeShotLinear(ProjectileData projectileData, float delay)
    {
        yield return new WaitForSeconds(delay);
        projectileCoordinator.SphericalShot(projectileData, (int) (1.2f * GameMode.difficulty));
    }
    private IEnumerator RapidFireSpherical(ProjectileData projectileData, float delay, int loops)
    {
        for (int i = 0; i < loops; i++)
        {
            float delayPerLoop = delay / loops;
            yield return new WaitForSeconds(delayPerLoop);
            projectileCoordinator.SphericalShot(projectileData, (int) (1f * GameMode.difficulty));
        }
    }
    private IEnumerator ChargeShotRocketsSpherical(ProjectileData projectileData, float delay)
    {
        yield return new WaitForSeconds(delay);
        projectileData.projectileType = ProjectileType.Homing;
        projectileCoordinator.SphericalShot(projectileData, (int) (1.2f * GameMode.difficulty));
    }
    private IEnumerator ChargeShotRapidRockets(ProjectileData projectileData, float delay)
    {
        yield return new WaitForSeconds(delay);
        projectileData.projectileType = ProjectileType.Homing;
        projectileCoordinator.RapidFireShot(projectileData, (int) (0.95f * GameMode.difficulty), Vector2.up);
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
