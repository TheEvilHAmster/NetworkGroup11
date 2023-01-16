using System;
using System.Collections;
using System.Collections.Generic;
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
        spawner = GameObject.FindWithTag("NetworkManager").GetComponent<Alteruna.Spawner>();
        Projectile.OnLifeTimeExpired += DestroyProjectile;
    }

    public void DestroyProjectile(Projectile projectile)
    {
        spawner.Despawn(projectile.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ProjectileData projectileData;
            {
                projectileData.origin = transform.position;
                projectileData.velocity = Vector3.forward * 2;
                projectileData.projectileType = ProjectileType.Linear;
                projectileData.lifeTime = 9;
            }
            ShootShotgun(projectileData, 70, 6);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ProjectileData projectileData;
            {
                projectileData.origin = transform.position;
                projectileData.velocity = Vector3.forward * 2;
                projectileData.projectileType = ProjectileType.Linear;
                projectileData.lifeTime = 9;
            }
            SphericalShot(projectileData, 40, 5, 10);
        }
    }
    

    private void Spawn()
    {
        spawner.Spawn(indexToSpawn, transform.position, transform.rotation);
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

    private void SphericalShot(ProjectileData projectileData, int count, int gapWidth, int gapStep)
    {
        float speed = projectileData.velocity.magnitude;
        
        int gapCount = gapStep < 1 ? 0 : count / (gapStep + gapWidth); 
        
        float angularOffset = 360 / count;
        
        Vector3 normalizedVelocity = Vector3.forward;
        Vector3 axis = Vector3.Cross(normalizedVelocity, Vector3.right);
        
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
        Projectile projectile = spawner.Spawn(indexToSpawn, projectileData.origin, transform.rotation).GetComponent<Projectile>();
        projectile.Shoot(projectileData.origin, projectileData.velocity, projectileData.lifeTime);
    }
}
