using System;
using Alteruna;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 20f;
    
    private Vector3 velocity;
    
    public static Action<Projectile> OnLifeTimeExpired;
    public void Shoot(Vector3 origin, Vector3 velocity, float lifeTime)
    {
        transform.position = origin;
        this.velocity = velocity;
        enabled = true;
        gameObject.SetActive(true);
        //transform.rotation = Quaternion.LookRotation(velocity, Vector3.forward);
        this.lifeTime = lifeTime;
    }
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    private void Update()
    {
        UpdateMovement();
    }
    
    protected void UpdateMovement()
    {
        transform.position += velocity * Time.deltaTime;
    }

    private void OnDestroy()
    {
        //OnLifeTimeExpired.Invoke(this);
    }
}
