using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Avatar = Alteruna.Avatar;

public class Respawn : MonoBehaviour
{
    private bool dead = false;
    private double waitTime = 10;
    private double timeLeft = 0;
    private Alteruna.Avatar avatar;

    private int invincibilityTime = 2;
    private double invincibilityTimeLeft;
    private void Start()
    {
        avatar = GetComponent<Avatar>();
    }

    void Update()
    {
        if (dead && (timeLeft + waitTime - GameMode.gameStopWatch.Elapsed.TotalSeconds < 0))
        {
            Revive();
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (avatar.IsMe && !col.CompareTag("Border") && invincibilityTime + invincibilityTimeLeft - GameMode.gameStopWatch.Elapsed.TotalSeconds < 0)
        {
            Die();
            Debug.Log("hit!");
        }
    }
    private void Revive()
    {
        transform.position = new Vector3(10, -4f, 0);
        invincibilityTimeLeft = GameMode.gameStopWatch.Elapsed.TotalSeconds;
        dead = false;
    }
    public void Die()
    {
        transform.position = new Vector3(0, 5000, 0); //Goes to code heaven
        timeLeft = GameMode.gameStopWatch.Elapsed.TotalSeconds;
        dead = true;
    }
}
