using System;
using Alteruna;
using UnityEngine;

public class Projectile : Synchronizable
{
    private Vector3 velocity;
    private Vector3 oldVelocity = Vector3.zero;
    private float lifeTime;
    public static Action<Projectile> OnLifeTimeExpired;
    public void Shoot(Vector3 origin, Vector3 velocity, float lifeTime)
    {
        transform.position = origin;
        this.velocity = velocity;
        transform.rotation = Quaternion.LookRotation(velocity);
        this.lifeTime = lifeTime;
    }
    public override void DisassembleData(Reader reader, byte LOD)
    {
        // Set our data to the updated value we have recieved from another player.
        velocity = reader.ReadVector3();

        // Save the new data as our old data, otherwise we will immediatly think it changed again.
        oldVelocity = velocity;
    }

    public override void AssembleData(Writer writer, byte LOD)
    {
        // Write our data so that it can be sent to the other players in our playroom.
        writer.Write(velocity);
    }
    private void Start()
    {
        lifeTime = 10;
    }

    private void Update()
    {
        UpdateSyncedProperties();
        UpdateMovement();
        UpdateLifeTime();
        base.SyncUpdate();
    }

    private void UpdateSyncedProperties()
    {
        if (velocity != oldVelocity)
        {
            oldVelocity = velocity;
            Commit();
        }
    }
    protected void UpdateMovement()
    {
        transform.position += velocity * Time.deltaTime;
    }

    private void UpdateLifeTime()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0 )
        {
            OnLifeTimeExpired.Invoke(this);
        }  
    }
}
