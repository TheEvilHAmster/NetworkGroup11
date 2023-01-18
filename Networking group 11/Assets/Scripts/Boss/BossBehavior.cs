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

    private Vector2 targetLocation = new Vector2(10, -4);
    private float speed = 2f;

    private float timePassed = 0;
    private float timeMoved = 0;
    private float moveTime = 1f;
    private float waitTime = 0f;

    private Multiplayer multiplayer;

    [SerializeField] private ProjectileCoordinator projectileCoordinator;
    void Update()
    {
        timePassed += Time.deltaTime;
        
        MoveTowardsLocation(targetLocation);
        UpdateTargetLocation();
        
    }

    private void UpdateTargetLocation()
    {
        timeMoved += Time.deltaTime / moveTime;
        if (timeMoved > moveTime + waitTime)
        {
            targetLocation = GetRandomLocation();
            timeMoved = 0;
            ProjectileData projectileData;
            {
                projectileData.origin = transform.position;
                projectileData.velocity = Vector3.right * 1;
                projectileData.projectileType = ProjectileType.Linear;
                projectileData.lifeTime = 9;
            }
            projectileCoordinator.SphericalShot(projectileData, 30, 5, 10);
        }
    }
    private void MoveTowardsLocation(Vector3 location)
    {
        float t = Mathf.Clamp( timeMoved, 0f, 1f);
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
