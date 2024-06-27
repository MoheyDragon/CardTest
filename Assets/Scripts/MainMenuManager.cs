using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] LevelButton[] levels;
    private void Start()
    {
        int levelsToUnlock = SaveLoadManager.Singleton.LoadLevelReached();
        for (int i = 0; i < levelsToUnlock; i++)
            levels[i].UnlockLevel();
    }
    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}
