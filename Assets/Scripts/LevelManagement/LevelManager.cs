using System;
using System.Collections;
using UnityEngine;

public class LevelManager : Singletons<LevelManager>
{
    public int LevelIndex;
    public Action OnMatch;
    public Action <int>OnMisMatch;
    public Action OnLevelStart;
    public Action OnWin;
    public Action OnLose;
    public Action <int>OnTurn;

    [Space]
    [SerializeField] bool isLevelTimeLimited;
    [SerializeField] float levelTime=60;
    public Action <int>OnTimeUpdated;

    [Space]
    [SerializeField] bool isMatchMistakesLimited;
    [SerializeField] int mistakesLimit=5;
    int mistakesCount;

    private int matchesCount;
    private int totalMatches;

    private int turns;
    private void Start()
    {
        SetupValues();
        Invoke(nameof(StartLevel), CardManager.Singleton.ShowCardsDuration);
    }
    private void StartLevel()
    {
        if (isLevelTimeLimited)
            StartCoroutine(CO_StartMatchTimer());
        OnLevelStart?.Invoke();
    }
    private void SetupValues()
    {
        totalMatches = GridManager.Singleton.totalMatchesCount;
        mistakesCount = mistakesLimit;
    }
    public void CorrectMatch()
    {
        IncreaseTurns();
        matchesCount++;
        if (matchesCount == totalMatches)
            OnWin?.Invoke();
        else
            OnMatch?.Invoke();
    }
    public void WrongMatch()
    {
        IncreaseTurns();
        mistakesCount--;
        OnMisMatch?.Invoke(mistakesCount);
        if(isMatchMistakesLimited)
        {
            if(mistakesCount == 0)
                OnLose?.Invoke();
        }
    }
    private void IncreaseTurns()
    {
        turns++;
        OnTurn?.Invoke(turns);
    }
    IEnumerator CO_StartMatchTimer()
    {
        WaitForSeconds second = new WaitForSeconds(1);
        float elapsedTime = levelTime;
        OnTimeUpdated?.Invoke((int)elapsedTime);
        while (elapsedTime>0)
        {
            yield return second;
            elapsedTime --;
            OnTimeUpdated?.Invoke((int)elapsedTime);
        }
        OnLose?.Invoke();
    }
    #region ForUICouldBeDoneBetterIfHaveMoreTime
    public bool IsLoseTimeBased=> isLevelTimeLimited;
    public bool IsLoseMistakesBased=> isMatchMistakesLimited;
    public int totalMistakesAllowed => mistakesLimit;
    public float timeAllowed => levelTime;
    #endregion
}
