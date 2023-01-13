using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeKeeper : MonoBehaviour
{
    public float TimeBrr = 0;
    public float IncreaseInDifficultyInterval = 5;
    public bool startTimer = false;
    private bool flag = false;
    public int difficulty = 0;
    private float timer = 0;
    private bool timerOn = false;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            TimeBrr += Time.deltaTime;
        }

        if ((((int)TimeBrr % (int)IncreaseInDifficultyInterval)  != 0) && !flag)
        {
            flag = true;
        }

        if ((((int)TimeBrr % (int)IncreaseInDifficultyInterval) == 0) && flag)
        {
            flag = false;
            IncreaseDifficulty();
        }

        if (timerOn)
        {
            timer -= Time.deltaTime;
            if (timer == 0)
            {
                timerOn = false;
                TimerDone();
            }
        }
    }

    private void TimerDone()
    {
        throw new NotImplementedException();
    }

    void Timer(float _timer)
    {
        timer = _timer;
        timerOn = true;
    }

    private void IncreaseDifficulty()
    {
        difficulty++;
    }


    void StartTimer()
    {
        startTimer = true;
    }

    void StopTimer()
    {
        startTimer = false;
    }
}