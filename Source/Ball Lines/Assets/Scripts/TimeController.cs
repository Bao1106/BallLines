using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public static TimeController instance;

    public TextMeshProUGUI timeCounter;

    private TimeSpan timePlaying;
    private bool timerGoing;
    private float elapsedTime;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //timeCounter.text = "00:001";
        //timerGoing = false;
    }

    private IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timeStr = timePlaying.ToString("mm':'ss");
            timeCounter.text = timeStr;

            yield return null;
        }
    }

    public void BeginTimer()
    {
        timerGoing = true;
        elapsedTime = 0f;

        //UpdateTimer();
        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerGoing = false;
    }

}
