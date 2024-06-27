using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] LevelButton[] levels;
    private void Start()
    {
        UnlockLevels(GetLevelsToUnlock());
    }
    private void UnlockLevels(int levelsToUnlock)
    {
        for (int i = 0; i < levelsToUnlock; i++)
            levels[i].UnlockLevel();
    }
    int GetLevelsToUnlock()
    {
        int levelsToUnlock = SaveLoadManager.Singleton.LoadLevelReached() + 1;

        if (levelsToUnlock > SaveLoadManager.Singleton.totalLevelCounts)
            levelsToUnlock = SaveLoadManager.Singleton.totalLevelCounts;

        return levelsToUnlock;
    }
    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}
