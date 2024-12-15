using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CicloDeDia : MonoBehaviour
{
    [SerializeField] private static float duracionDiaEnSegundos = 480f;
    private static readonly TimeSpan workDayStartTime = new TimeSpan(10, 0, 0);
    private static readonly TimeSpan workDayEndTime = new TimeSpan(24, 0, 0);

    private static float elapsedTime;

    public int diaActual = 1;

    public static Action finishDay;

    private void Start()
    {
        elapsedTime = 0f;
    }
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > duracionDiaEnSegundos)
        {
            elapsedTime = 0f;
            diaActual++;
            finishDay?.Invoke();
        }
    }

    private static TimeSpan getTime() 
    {
        float progress = elapsedTime / duracionDiaEnSegundos;
        double totalWorkDaySeconds = (workDayEndTime - workDayStartTime).TotalSeconds;
        double simulatedSeconds = totalWorkDaySeconds * progress;
        return workDayStartTime.Add(TimeSpan.FromSeconds(simulatedSeconds));
    }

    public static TimeSpan getCurrentTime()
    {
        TimeSpan currentTime = getTime();
        return currentTime;
    }

    public static int howMuchTimePassed(TimeSpan time1)
    {
        TimeSpan difference = getCurrentTime() - time1;
        return Math.Abs((int)difference.TotalMinutes);
    }
}
