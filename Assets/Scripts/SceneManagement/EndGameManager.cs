using UnityEngine;
using UnityEngine.SceneManagement;
public class EndGameManager : Singletons<EndGameManager>
{
    [SerializeField] Canvas gameEndCanvas;
    [Space]
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private GameObject menuButton;
    
    private void Start()
    {
        Setup();
        SubscribeToLevelManager();
    }
    public void PlayNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void Restart()
    {
        SceneManager.LoadScene(LevelManager.Singleton.LevelIndex);
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void Setup()
    {
        gameEndCanvas.enabled= false;
        restartButton.SetActive(false);
        menuButton.SetActive(false);
        nextLevelButton.SetActive(false);
    }
    private void SubscribeToLevelManager()
    {
        LevelManager.Singleton.OnWin += OnWin;
        LevelManager.Singleton.OnLose += OnLose;
    }
    private void OnWin()
    {
        gameEndCanvas.enabled = true;
        if (LevelManager.Singleton.LevelIndex == SaveLoadManager.Singleton.totalLevelCounts)
            ShowButtons(new GameObject[] { menuButton});
        else
            ShowButtons(new GameObject[] { menuButton,nextLevelButton});
    }
    private void OnLose()
    {
        gameEndCanvas.enabled = true;
        ShowButtons(new GameObject[] { menuButton,restartButton});
    }
    private void ShowButtons(GameObject[] buttons)
    {
        foreach(GameObject button in buttons)
        {
            button.SetActive(true);
        }
    }
}
