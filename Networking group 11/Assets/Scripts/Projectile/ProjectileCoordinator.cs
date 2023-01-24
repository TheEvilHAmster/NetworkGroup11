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

        float angularOffset = 360 / count;
        
        Vector3 normalizedVelocity = Vector3.right;
        Vector3 axis = Vector3.Cross(normalizedVelocity, Vector3.up);
        
        for (int i = 0; i < count; i++)
        {
            projectileData.velocity = Quaternion.AngleAxis(angularOffset * i, axis) * normalizedVelocity * speed;
            Shoot(projectileData);
        }
    }
    public void SquareShot(ProjectileData projectileData, int count)
    {
        float speed = projectileData.velocity.magnitude;

        float angularOffset = 360 / count;
        
        Vector3 normalizedVelocity = Vector3.right;
        Vector3 axis = Vector3.Cross(normalizedVelocity, Vector3.up);

        Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };
        int dividedCount = count / 2;
        float offset = 0.25f;
        for (int i = 0; i < directions.Length; i++)
        {
            for (int y = 0; y < dividedCount; y++)
            {
                projectileData.velocity = directions[i] * speed;
                Vector3 offsetVector = directions[(i + 1) % directions.Length ];
                projectileData.origin += -offsetVector * offset * (dividedCount - y) + offsetVector * y * offset;
                Shoot(projectileData);
            }
        }
    }
    private void ShootProjectiles(ProjectileData[] projectileData)
    {
        for (int i = 0; i < projectileData.Length; i++)
        {
            Shoot(projectileData[i]);
        }
    }
    //private void Shoot(ProjectileData projectileData)
    //{
    //    Projectile projectile = spawner.Spawn(indexToSpawn, projectileData.origin, transform.rotation, new Vector3(0.3f, 0.3f, 0.3f)).GetComponent<Projectile>();
    //    projectile.Shoot(projectileData.origin, projectileData.velocity, projectileData.lifeTime);
    //}
    
    private void Shoot(ProjectileData projectileData)
    {
        Projectile projectile = ProjectilePool.Instance.GetObject();
        projectile.Shoot(projectileData.origin, projectileData.velocity, projectileData.lifeTime);
    }
}
