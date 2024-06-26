using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singletons<LevelManager>
{
    public Action OnWin;
    public Action OnLose;

    public Action <int>OnTimeUpdated;
    [SerializeField] bool isLevelTimeLimited;
    [SerializeField] float levelTime=60;

    [Space]
    public Action OnMistake;
    [SerializeField] bool isMatchMistakesLimited;
    [SerializeField] int mistakesLimit=5;
    int mistakesCount;

    private int matchesCount;
    private int totalMatches;
    protected override void Awake()
    {
        base.Awake();
        SubscribeToMatchActions();
        SetupValues();
    }
    private void Start()
    {
        if (isLevelTimeLimited)
            StartCoroutine(CO_StartMatchTimer());
    }

    private void SubscribeToMatchActions()
    {
        Card.OnMatching += OnMatchCorrect;
        if(isMatchMistakesLimited)
            Card.OnMissMatching += OnMissMatch;
    }
    private void SetupValues()
    {
        totalMatches = GridManager.Singleton.totalMatchesCount;
    }
    private void OnMatchCorrect()
    {
        matchesCount++;
        if (matchesCount == totalMatches)
            OnWin?.Invoke();
    }
    private void OnMissMatch()
    {
        mistakesCount++;
            OnMistake?.Invoke();
        if(mistakesCount == mistakesLimit)
            OnLose?.Invoke();
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
    }
    private void OnDisable()
    {
        UnSubscribeToMatchActions();
    }
    private void UnSubscribeToMatchActions()
    {
        Card.OnMatching -= OnMatchCorrect;
        if (isMatchMistakesLimited)
            Card.OnMissMatching -= OnMissMatch;
    }
}
