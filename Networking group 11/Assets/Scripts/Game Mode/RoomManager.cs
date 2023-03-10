using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using Alteruna;
using Alteruna.Trinity;
using UnityEngine;
using Avatar = UnityEngine.Avatar;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Multiplayer multiplayer;
    [SerializeField] private int minPlayers = 1;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject startGameButton;

    [SerializeField] private StandardGameMode gameModePrefab;
    [SerializeField] private GameObject loseUI;
    [SerializeField] private GameObject winUI;
    private int playersOnline;
    private bool gameStarted;
    public static readonly List<PlayerMovement> players = new List<PlayerMovement>();

    private StandardGameMode gameMode;
    void Start()
    {
        multiplayer.RegisterRemoteProcedure("GameStarted", ReceiveGameStarted);
        startGameButton.SetActive(false);
        multiplayer.RoomJoined.AddListener(SetPlayers);
    }
    private void SetPlayers(Multiplayer arg0, Room arg1, User arg2)
    {
        startGameButton.SetActive(true);
    }

    public void CheckLose()
    {
        gameMode.CheckLose();
    }
    private void ReceiveGameStarted(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        if (!gameStarted)
        {
            parameters.Get("time", out string time);
            Debug.Log(time);
            DateTime networkedTime = DateTime.ParseExact(time, "yyyy.MM.dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
            DateTime myTime = GetNetworkTime();
            TimeSpan timeSpan = myTime - networkedTime;
            float deltaTime = (float)timeSpan.TotalMilliseconds / 1000f;
            GameMode.timeTilGameStart += deltaTime;
            StartGame();
        }
    }
    private void StartGame()
    {
        Debug.Log("Game Started");
        roomPrefab.SetActive(false);
        startGameButton.SetActive(false);
        gameStarted = true;
        players.AddRange(FindObjectsOfType<PlayerMovement>());
        gameMode = Instantiate(gameModePrefab, transform.position, Quaternion.identity);
        players.Sort(SortPlayersByInstanceID);
        loseUI.SetActive(false);
        winUI.SetActive(false);
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<Respawn>().Revive();
        }
    }
    public void OnButtonPress()
    {
        Debug.Log("pressed");
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
            DateTime dateTime = GetNetworkTime();
            parameters.Set("time", dateTime.ToString("yyyy.MM.dd HH:mm:ss:fff"));
            Debug.Log("Minutes " + dateTime.Minute + " Seconds: " + dateTime.Second + " Milliseconds: " + dateTime.Millisecond);
            multiplayer.InvokeRemoteProcedure("GameStarted", UserId.All, parameters);
            
            StartGame();
        }
    }
    
    public void ResetGame()
    {
        gameStarted = false;
        roomPrefab.SetActive(true);
        startGameButton.SetActive(false);
    }
    public void Lose()
    {
        loseUI.SetActive(true);
        ResetGame();
    }

    public void Win()
    {
        ResetGame();
        winUI.SetActive(true);
    }
    public static int SortPlayersByInstanceID(PlayerMovement o1, PlayerMovement o2) {
        return o1.GetInstanceID().CompareTo(o2.GetInstanceID());
    }
    public static DateTime GetNetworkTime()
    {
        const string ntpServer = "time.windows.com";
        
        var ntpData = new byte[48];
        
        ntpData[0] = 0x1B;

        var addresses = Dns.GetHostEntry(ntpServer).AddressList;
        
        var ipEndPoint = new IPEndPoint(addresses[0], 123);

        using(var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            socket.Connect(ipEndPoint);
            
            socket.ReceiveTimeout = 3000;     

            socket.Send(ntpData);
            socket.Receive(ntpData);
            socket.Close();
        }
        
        const byte serverReplyTime = 40;

        ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

        ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);
        
        intPart = SwapEndianness(intPart);
        fractPart = SwapEndianness(fractPart);

        var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
        
        var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

        return networkDateTime.ToLocalTime();
    }
    
    static uint SwapEndianness(ulong x)
    {
        return (uint) (((x & 0x000000ff) << 24) +
                       ((x & 0x0000ff00) << 8) +
                       ((x & 0x00ff0000) >> 8) +
                       ((x & 0xff000000) >> 24));
    }
}
