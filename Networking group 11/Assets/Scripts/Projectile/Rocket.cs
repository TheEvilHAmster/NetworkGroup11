
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rocket : Projectile
{
    private Vector3 target;
    private float rotateSpeed = 80f;
    private float speed;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;

    

    protected override void UpdateProjectile()
    {
        UpdateMovement();
    }

    protected override void StartProjectile() 
    {
        speed = velocity.magnitude * 1.5f;
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = velocity;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        float rotationZ = Mathf.Atan2(-velocity.x, velocity.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        rigidbody.position += rigidbody.velocity * 1.5f;
        ReturnToObjectPool(lifeTime);
    }

    protected override IEnumerator ReturnToPoolDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RocketPool.Instance.ReturnToPool(this);
    }

    public void SetTarget(Vector3 newTarget)
    {
        target = newTarget;
    }

    private void UpdateMovement()
    {
        Vector2 direction = transform.position - target;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rigidbody.angularVelocity = rotateSpeed * rotateAmount;
        rigidbody.velocity = transform.up * speed;
    }
}
