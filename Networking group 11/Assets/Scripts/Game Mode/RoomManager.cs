using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Alteruna.Trinity;
using UnityEngine;
using Avatar = UnityEngine.Avatar;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Multiplayer multiplayer;
    [SerializeField] private int minPlayers = 2;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject startGameButton;

    [SerializeField] private StandardGameMode gameMode;
    private int playersOnline;
    private bool gameStarted;
    
    void Start()
    {
        multiplayer.RegisterRemoteProcedure("GameStarted", SendGameStarted);
        startGameButton.SetActive(false);
        multiplayer.RoomJoined.AddListener(SetPlayers);
    }
    private void SetPlayers(Multiplayer arg0, Room arg1, User arg2)
    {
        startGameButton.SetActive(true);
    }
    private void SendGameStarted(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        float networkedSeconds = parameters.Get("seconds", 0) +
                                 (float)parameters.Get("milliseconds", 0) / 1000f;
        float deltaSeconds = GetDeltaSeconds(networkedSeconds);
        GameMode.timePassedSinceStart += deltaSeconds;
        Debug.Log("Networked seconds :" + networkedSeconds);
        Debug.Log("Delta seconds " + deltaSeconds);
        Debug.Log("milli: " + parameters.Get("milliseconds", 0));
        StartGame();
    }

    private float GetDeltaSeconds(float seconds)
    {
        DateTime dateTime = GetDateTime();
        float mySeconds = dateTime.Second + (float)dateTime.Millisecond / 1000f + dateTime.Minute * 60;
        Debug.Log("Current seconds :" + mySeconds);
        float deltaSeconds = mySeconds - seconds;
        return deltaSeconds;
    }
    private void StartGame()
    {
        Debug.Log("Game Started");
        roomPrefab.SetActive(false);
        startGameButton.SetActive(false);
        gameStarted = true;
        Instantiate(gameMode, transform.position, Quaternion.identity);
    }
    public void OnButtonPress()
    {
        if (!gameStarted)
        {
            TryStartGame();
        }
    }
    private void TryStartGame()
    {
        if (multiplayer.GetUsers().Count >= minPlayers)
        {
            ProcedureParameters parameters = new ProcedureParameters();
            DateTime dateTime = GetDateTime();
            parameters.Set("milliseconds", dateTime.Millisecond);
            parameters.Set("seconds", dateTime.Second);
            parameters.Set("minutes", dateTime.Minute);
            Debug.Log("Minutes " + dateTime.Minute + " Seconds: " + dateTime.Second + " Milliseconds: " + dateTime.Millisecond);
            multiplayer.InvokeRemoteProcedure("GameStarted", UserId.All, parameters);
            StartGame();
        }
    }
    public static DateTime GetDateTime()
    {
        DateTime dateTime = DateTime.MinValue;
        System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://www.microsoft.com");
        request.Method ="GET";
        request.Accept ="text/html, application/xhtml+xml, */*";
        request.UserAgent ="Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
        request.ContentType ="application/x-www-form-urlencoded";
        request.CachePolicy =new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
        System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            string todaysDates = response.Headers["date"];
 
            dateTime = DateTime.ParseExact(todaysDates,"ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat, System.Globalization.DateTimeStyles.AssumeUniversal);
        }
        return dateTime;
    }
}
