using UnityEngine;
using System.Collections;
using System;
public class ScoreManager:Singletons<ScoreManager>
{
    public Action OnCombo;
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
    protected override void Awake()
    {
        base.Awake();
        timeInSeconds=new WaitForSeconds(waitTimeForCombo);
        SubscribeToCardActions();
    }
    private void SubscribeToCardActions()
    {
        Card.OnMatching += OnMatching;
        Card.OnMissMatching += OnMissMatching;

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
            isComboTimerActive = false;
    }
    private void OnMissMatching()
    {
        if (isComboAffectedByMistakes)
            isLastMatchWin = false;

    }
    private void AddScore()
    {
        score++;
        print("Score = " + score);
    }
    private void OnComboAchieved()
    {
        comboStrike++;
        print("Combo : " + comboStrike);
        OnCombo?.Invoke();
        AddScore();
    }
}