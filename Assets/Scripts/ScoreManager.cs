using UnityEngine;
using System.Collections;
using System;
public class ScoreManager:Singletons<ScoreManager>
{
    public Action <int>OnCombo;
    public Action <int>OnScore;
    int score;
    int comboStrike;
    [Header("Time Based Combo")]
    [SerializeField] bool isComboAffectedByTime;
    [SerializeField] float waitTimeForCombo;
                     WaitForSeconds timeInSeconds;
                     bool isComboTimerActive;
    [Space]
    [Header("Mistakes Based Combo")]
    [SerializeField] bool isComboAffectedByMistakes;
                     bool isLastMatchWin;
    private void Start()
    {
        timeInSeconds = new WaitForSeconds(waitTimeForCombo);
        SubscribeToCardActions();
    }
    private void SubscribeToCardActions()
    {
        LevelManager.Singleton.OnMatch += OnMatching;
        LevelManager.Singleton.OnMisMatch += OnMissMatching;

    }
    private void OnMatching()
    {
        AddScore();
        CheckCombo();
        PrepareForNextCombo();
    }
    private void CheckCombo()
    {
        bool isComboAchieved = false;

        if (isComboAffectedByMistakes)
            isComboAchieved = CheckMistakesBasedCombo();

        if (isComboAffectedByTime)
            isComboAchieved = CheckTimedBaseCombo();

        if (isComboAchieved)
            OnComboAchieved();
        else
            comboStrike = 0;
    }
    private void ResetCombo()
    {
        comboStrike =-1;
        OnCombo?.Invoke(comboStrike);
    }
    private void PrepareForNextCombo()
    {
        if(isComboAffectedByMistakes)
            isLastMatchWin = true;
        if (isComboAffectedByTime)
            RestartTimer();

    }
    private bool CheckMistakesBasedCombo()
    {
        if (isLastMatchWin)
        {
            if(isComboAffectedByTime)
            {
                if (isComboTimerActive)
                    return true;
            }
            else
            {
                return true;
            }

        }
        return false;
    }
    private bool CheckTimedBaseCombo()
    {
        if (isComboTimerActive)
        {
            if (isComboAffectedByMistakes)
                return CheckMistakesBasedCombo();
            else
                return true;
        }
        return false;
    }

    Coroutine timerCoroutine;
    private void RestartTimer()
    {
        if(timerCoroutine!=null)
            StopCoroutine(timerCoroutine);
        timerCoroutine= StartCoroutine(CO_StartTimer());
    }
    int actionIndex;
    IEnumerator CO_StartTimer()
    {
        actionIndex++;
        isComboTimerActive = true;
        int thisActionIndex = actionIndex;
        yield return timeInSeconds;
        if (actionIndex == thisActionIndex)
        {
            isComboTimerActive = false;
            ResetCombo();
        }
    }
    private void OnMissMatching(int mistakesCount)
    {
        if (isComboAffectedByMistakes)
        {
            isLastMatchWin = false;
            ResetCombo();
        }
    }
    private void AddScore()
    {
        score++;
        OnScore?.Invoke(score);
    }
    private void OnComboAchieved()
    {
        AddScore();
        comboStrike++;
        OnCombo?.Invoke(comboStrike);
    }
}