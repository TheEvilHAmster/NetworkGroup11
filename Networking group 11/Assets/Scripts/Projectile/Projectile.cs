using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 velocity;
    private float lifeTime;
    public static Action<Projectile> OnLifeTimeExpired;
    public void Shoot(Vector3 origin, Vector3 velocity, float lifeTime)
    {
        transform.position = origin;
        this.velocity = velocity;
        transform.rotation = Quaternion.LookRotation(velocity);
        this.lifeTime = lifeTime;
    }
    
    private void Update()
    {
        UpdateMovement();
        UpdateLifeTime();
    }
    protected void UpdateMovement()
    {
        transform.position += velocity * Time.deltaTime;
    }

    private void UpdateLifeTime()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            OnLifeTimeExpired.Invoke(this);
        }  
    }
}
