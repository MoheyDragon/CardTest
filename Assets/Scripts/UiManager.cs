using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UiManager : Singletons<UiManager>
{
    [SerializeField] TextMeshProUGUI levelName;
    [SerializeField] TextMeshProUGUI turns;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI combo;
    [Space]
    [SerializeField] TextMeshProUGUI lose;
    [SerializeField] TextMeshProUGUI loseStaticText;

    private void Start()
    {
        SubscribeToActions();
        SetLoseMeter();
        ResetUiTextsValues();
    }
    private void SubscribeToActions()
    {
        LevelManager.Singleton.OnTurn += UpdateTurns;
        ScoreManager.Singleton.OnScore += UpdateScore;
        ScoreManager.Singleton.OnCombo += UpdateCombo;
    }
    private void ResetUiTextsValues()
    {
        levelName.text = LevelManager.Singleton.LevelIndex.ToString();
        UpdateCombo(0);
        UpdateScore(0);
        UpdateTurns(0);
    }
    private void UpdateScore(int newValue)
    {
        UpdateText(score, newValue);
    }
    private void UpdateCombo(int newValue)
    {
        UpdateText(combo, newValue);
    }
    private void UpdateTurns(int newValue)
    {
        UpdateText(turns, newValue);
    }
    // For now Game Supports 2 cases for losing , time based and mistakes count based, but for Ui I didn't have time to implement this dynamically also,
    // so it's better to only have one of the 2 values true or none 
    private void SetLoseMeter()
    {
        if (LevelManager.Singleton.IsLoseTimeBased)
        {
            loseStaticText.text = "Time :";
            lose.text = LevelManager.Singleton.timeAllowed.ToString();
            LevelManager.Singleton.OnTimeUpdated += UpdateLose;
        }
        else if (LevelManager.Singleton.IsLoseMistakesBased)
        {
            loseStaticText.text = "Tries :";
            lose.text = LevelManager.Singleton.totalMistakesAllowed.ToString();
            LevelManager.Singleton.OnMisMatch += UpdateLose;
        }
        else
        {
            Destroy(loseStaticText.gameObject);
        }
    }
    private void UpdateLose(int newValue)
    {
        UpdateText(lose, newValue);
    }
    private void UpdateText(TextMeshProUGUI text,int newValue)
    {
        text.text= newValue.ToString();
    }
}
