using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public enum Enemy
{
    SphereSpawner = 1,
    
}
public class BossSpawner : MonoBehaviour
{
    private Spawner spawner;
    [SerializeField] private BossBehavior boss;
    private void Awake()
    {
        spawner = GameObject.FindWithTag("NetworkManager").GetComponent<Spawner>();
    }
    public void Spawn()
    {
        //spawner.Spawn(1, transform.position);
        Instantiate(boss, transform.position, Quaternion.identity);
    }
}
