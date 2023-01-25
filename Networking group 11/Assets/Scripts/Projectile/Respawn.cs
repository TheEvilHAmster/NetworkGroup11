using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Alteruna.Trinity;
using UnityEngine;
using Avatar = Alteruna.Avatar;

public class Respawn : MonoBehaviour
{
    public bool dead = false;
    private double waitTime = 10;
    private double timeLeft = 0;
    private Alteruna.Avatar avatar;

    private int invincibilityTime = 2;
    private double invincibilityTimeLeft;
    private Multiplayer multiplayer;
    private void Start()
    {
        avatar = GetComponent<Avatar>();
        multiplayer = FindObjectOfType<Multiplayer>();
        multiplayer.RegisterRemoteProcedure("Die", Die);
        multiplayer.RegisterRemoteProcedure("Revive", Revive);
    }
    

    void Update()
    {
        if (avatar.IsMe && dead && (timeLeft + waitTime - GameMode.gameStopWatch.Elapsed.TotalSeconds < 0))
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
        dead = false;
        transform.position = new Vector3(10, -4f, 0);
        invincibilityTimeLeft = GameMode.gameStopWatch.Elapsed.TotalSeconds;
        ProcedureParameters parameters = new ProcedureParameters();
        multiplayer.InvokeRemoteProcedure("Revive", UserId.All, parameters);
        AddPlayerAsTarget();
    }
    private void Die()
    {
        dead = true;
        transform.position = new Vector3(0, 5000, 0); //Goes to code heaven
        timeLeft = GameMode.gameStopWatch.Elapsed.TotalSeconds;
        ProcedureParameters parameters = new ProcedureParameters();
        multiplayer.InvokeRemoteProcedure("Die", UserId.All, parameters);
        RemovePlayerAsTarget();
    }
    private void Revive(ushort fromuser, ProcedureParameters parameters, uint callid, ITransportStreamReader processor)
    {
        dead = false;
        AddPlayerAsTarget();
    }
    public void Die(ushort fromuser, ProcedureParameters parameters, uint callid, ITransportStreamReader processor)
    {
        dead = true;
        RemovePlayerAsTarget();
    }
    
    private void RemovePlayerAsTarget()
    {
        RoomManager.players.Remove(GetComponent<PlayerMovement>());
        Debug.Log(RoomManager.players.Count);
    }

    private void AddPlayerAsTarget()
    {
        RoomManager.players.Add(GetComponent<PlayerMovement>());
        RoomManager.players.Sort(RoomManager.SortPlayersByInstanceID);
        Debug.Log(RoomManager.players.Count);
    }
}
