using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Unity.VisualScripting;
using UnityEngine;

public enum ProjectileType
{
    Linear,
    Homing,
}
public class ProjectileCoordinator : MonoBehaviour
{
    [SerializeField] private Alteruna.Avatar avatar;
    [SerializeField] private Alteruna.Spawner spawner;
    [SerializeField] private int indexToSpawn = 0;
    private void Awake()
    {
        avatar = GetComponent<Alteruna.Avatar>();
        spawner = GameObject.FindWithTag("NetworkManager").GetComponent<Spawner>();
        Projectile.OnLifeTimeExpired += DestroyProjectile;
    }

    public void DestroyProjectile(Projectile projectile)
    {
        spawner.Despawn(projectile.gameObject);
    }

    private void Update()
    {
        
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

    public void SphericalShot(ProjectileData projectileData, int count, int gapWidth, int gapStep)
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
    private void ShootProjectiles(ProjectileData[] projectileData)
    {
        for (int i = 0; i < projectileData.Length; i++)
        {
            Shoot(projectileData[i]);
        }
    }
    private void Shoot(ProjectileData projectileData)
    {
        Projectile projectile = spawner.Spawn(indexToSpawn, projectileData.origin, transform.rotation, new Vector3(0.3f, 0.3f, 0.3f)).GetComponent<Projectile>();
        projectile.Shoot(projectileData.origin, projectileData.velocity, projectileData.lifeTime);
    }
}
