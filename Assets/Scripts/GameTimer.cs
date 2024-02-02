using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    private float timeElapsed = 0f;
    public Text timerTxt;

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60f);
        int seconds = Mathf.FloorToInt(timeElapsed % 60f);
        int ms = Mathf.FloorToInt((timeElapsed * 1000) % 100);

        timerTxt.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, ms);
    }
}
