using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singletons<LevelManager>
{
    public Action OnMatch;
    public Action <int>OnMisMatch;
    public Action OnLevelStart;
    public Action OnWin;
    public Action OnLose;

    public Action <int>OnTimeUpdated;
    [SerializeField] bool isLevelTimeLimited;
    [SerializeField] float levelTime=60;

    [Space]
    [SerializeField] bool isMatchMistakesLimited;
    [SerializeField] int mistakesLimit=5;
    int mistakesCount;

    private int matchesCount;
    private int totalMatches;
    private void Start()
    {
        SetupValues();
        if (isLevelTimeLimited)
            StartCoroutine(CO_StartMatchTimer());
        Invoke(nameof(StartLevel), 1);
    }
    private void StartLevel()
    {
        OnLevelStart?.Invoke();
    }
    private void SetupValues()
    {
        totalMatches = GridManager.Singleton.totalMatchesCount;
    }
    public void CorrectMatch()
    {
        matchesCount++;
        if (matchesCount == totalMatches)
            OnWin?.Invoke();
        else
            OnMatch?.Invoke();
    }
    public void WrongMatch()
    {
        mistakesCount++;
        OnMisMatch?.Invoke(mistakesCount);
        if(isMatchMistakesLimited)
        {
            if(mistakesCount == mistakesLimit)
                OnLose?.Invoke();
        }
    }
    IEnumerator CO_StartMatchTimer()
    {
        WaitForSeconds second = new WaitForSeconds(1);
        float elapsedTime = 0;
        while(elapsedTime<=levelTime)
        {
            yield return second;
            elapsedTime += 1;
            OnTimeUpdated?.Invoke((int)elapsedTime);
        }
        OnLose?.Invoke();
    }
}
