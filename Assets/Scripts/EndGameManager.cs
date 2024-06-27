using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class EndGameManager : Singletons<EndGameManager>
{
    [SerializeField] Canvas gameEndCanvas;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private GameObject menuButton;
    const int totalLevelCounts = 5;
    private void Start()
    {
        Setup();
        SubscribeToActions();
    }
    public void PlayNextLevel()
    {
        SceneManager.LoadScene(LevelManager.Singleton.LevelIndex++);
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
    private void SubscribeToActions()
    {
        LevelManager.Singleton.OnWin += OnWin;
        LevelManager.Singleton.OnLose += OnLose;
    }
    private void OnWin()
    {
        gameEndCanvas.enabled = true;
        if (LevelManager.Singleton.LevelIndex == totalLevelCounts)
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