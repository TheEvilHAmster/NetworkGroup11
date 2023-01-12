using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeKeeper : MonoBehaviour
{
    private Time time;
    private TimeKeeper timeKeeper;
    private float TimeBrr = 0;
    private double TimeBRRRRR = 0;
    private float IncreaseInDifficultyInterval = 5;
    private bool startTimer = false;
    private bool flag = false;
    private int difficulty = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            TimeBrr += Time.deltaTime;
            TimeBRRRRR += Time.deltaTime;
        }

        if ((int)IncreaseInDifficultyInterval % (int)TimeBrr != 0 && !flag)
        {
            flag = true;
        }
        if ((int)IncreaseInDifficultyInterval % (int)TimeBrr == 0 && flag)
        {
            flag = false;
            IncreaseDifficulty();
        }
        
    }


    private void IncreaseDifficulty()
    {
        throw new NotImplementedException();
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
