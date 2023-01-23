using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform teleportLocation;
    [SerializeField] private bool useX;
    [SerializeField] private bool useY;
    private void OnTriggerEnter2D(Collider2D col)
    {
        Vector3 targetLocation = teleportLocation.transform.position;
        Vector3 currentLocation = col.transform.position;
        Vector2 finalTarget =  new Vector2( useX ? targetLocation.x : currentLocation.x, useY ? targetLocation.y : currentLocation.y);
        col.transform.position = finalTarget;
    }
}
