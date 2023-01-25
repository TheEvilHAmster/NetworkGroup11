using System;
using Alteruna;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 20f;
    
    protected Vector3 velocity;
    
    public static Action<Projectile> OnLifeTimeExpired;
    [SerializeField] private float ProjectileOffset = 1.7f;
    public void Shoot(Vector3 origin, Vector3 velocity, float lifeTime)
    {
        transform.position = origin;
        this.velocity = velocity;
        enabled = true;
        gameObject.SetActive(true);
        
        //transform.rotation = Quaternion.LookRotation(velocity, Vector3.forward);
        this.lifeTime = lifeTime;
        
        StartProjectile();
    }
    private void Start()
    {
        Destroy(gameObject, lifeTime);
        
        transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        
    }

    protected virtual void StartProjectile()
    {
        transform.position += velocity * ProjectileOffset;
    }
    private void Update()
    {
        UpdateProjectile();
    }
    
    protected virtual void UpdateProjectile()
    {
        transform.position += velocity * Time.deltaTime;
    }

    private void OnDestroy()
    {
        //OnLifeTimeExpired.Invoke(this);
    }
}
