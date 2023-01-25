using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ProjectileType
{
    Linear,
    Homing,
}
public class ProjectileCoordinator : MonoBehaviour
{
    [SerializeField] private Alteruna.Spawner spawner;
    [SerializeField] private Projectile projectilePrefab;
    
    private Vector2 topLeftCorner = new(-0.59f, 0.52f);
    private Vector2 bottomRightCorner = new (20.7f, -9.5f);
    private void Awake()
    {
        spawner = GameObject.FindWithTag("NetworkManager").GetComponent<Spawner>();
        Projectile.OnLifeTimeExpired += DestroyProjectile;
    }

    public void DestroyProjectile(Projectile projectile)
    {
        spawner.Despawn(projectile.gameObject);
    }
    private void ShootShotgun(ProjectileData projectileData, int count, float angularOffset)
    {
        float speed = projectileData.velocity.magnitude;
        Vector3 normalizedVelocity = projectileData.velocity;
        Vector3 axis = Vector3.Cross(normalizedVelocity, Vector3.right);
        for (int i = 0; i < count; i++)
        {
            projectileData.velocity = Quaternion.AngleAxis(angularOffset * i, axis) * normalizedVelocity * speed;
            Shoot(projectileData);
        }
    }

    public void SphericalShot(ProjectileData projectileData, int count)
    {
        float speed = projectileData.velocity.magnitude;
        count = Mathf.Clamp(count, 0, 360);
        float angularOffset = 360f / count;
        
        Vector3 normalizedVelocity = Vector3.right;
        Vector3 axis = Vector3.Cross(normalizedVelocity, Vector3.up);
        
        for (int i = 0; i < count; i++)
        {
            projectileData.velocity = Quaternion.AngleAxis(angularOffset * i, axis) * normalizedVelocity * speed;
            Shoot(projectileData);
        }
    }

    public void RapidSphericalShot(ProjectileData projectileData, int count)
    {
        StartCoroutine(RapidFireSphere(projectileData, count, 1.25f));
    }
    private IEnumerator RapidFireSphere(ProjectileData projectileData, int count, float totalDelay)
    {
        float speed = projectileData.velocity.magnitude;
        count = Mathf.Clamp(count, 0, 360);
        float angularOffset = 360f / count;
        
        Vector3 normalizedVelocity = Vector3.right;
        Vector3 axis = Vector3.Cross(normalizedVelocity, Vector3.up);
        float delay = totalDelay / count;
        for (int i = 0; i < count; i++)
        {
            projectileData.velocity = Quaternion.AngleAxis(angularOffset * i, axis) * normalizedVelocity * speed;
            yield return new WaitForSeconds(delay);
            Shoot(projectileData);
        }
    }
    public void RapidFireShot(ProjectileData projectileData, int count, Vector3 direction)
    {
        projectileData.velocity = direction * projectileData.velocity.magnitude;
        StartCoroutine(Fire(projectileData, count, 0.55f));
    }

    private IEnumerator Fire(ProjectileData projectileData, int count, float totalDelay)
    {
        float delay = totalDelay / count;
        
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(delay);
            Shoot(projectileData);
            
        }
    }
    private void ShootProjectiles(ProjectileData[] projectileData)
    {
        for (int i = 0; i < projectileData.Length; i++)
        {
            Shoot(projectileData[i]);
        }
    }
    private Vector2 GetRandomLocation()
    {
        return new Vector2(Random.Range(topLeftCorner.x, bottomRightCorner.x),
            Random.Range(bottomRightCorner.y, topLeftCorner.y));
    }
    
    private void Shoot(ProjectileData projectileData)
    {
        switch (projectileData.projectileType)
        {
            case ProjectileType.Linear :
                Projectile projectile = ProjectilePool.Instance.GetObject();
                projectile.Shoot(projectileData.origin, projectileData.velocity, projectileData.lifeTime);
                break;
            case ProjectileType.Homing :
                Rocket rocket = RocketPool.Instance.GetObject();
                rocket.Shoot(projectileData.origin, projectileData.velocity, projectileData.lifeTime);
                rocket.SetTarget(GetRandomLocation());
                break;
        }
        
    }
}
