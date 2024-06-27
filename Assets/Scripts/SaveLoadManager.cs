using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : Singletons<SaveLoadManager>
{
    string levelReachedPlayerPref="Level Reached";
    [HideInInspector]
    public int totalLevelCounts = 5;
    private void Start()
    {
        LoadLevelReached();
        if(LevelManager.Singleton)
            LevelManager.Singleton.OnWin += SaveLevelReached;
    }
    public int LoadLevelReached()
    {
        return PlayerPrefs.GetInt(levelReachedPlayerPref, 0);
    }
    private void SaveLevelReached()
    {
        int newLevelFinished = LevelManager.Singleton.LevelIndex;
        int lastLevelReached = LoadLevelReached();
        if (lastLevelReached > newLevelFinished) return;
        PlayerPrefs.SetInt(levelReachedPlayerPref, newLevelFinished);
    }
}
