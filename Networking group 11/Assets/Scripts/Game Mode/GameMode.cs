using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Alteruna;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public static float timeTilGameStart = 0f;
    public static readonly Stopwatch gameStopWatch = new ();
    public static int difficulty = 1;
    [SerializeField] protected float startGameSeconds = 3;
    
}
