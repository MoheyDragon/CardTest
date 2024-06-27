using UnityEngine;
using UnityEngine.UI;

public class LevelButton:MonoBehaviour
{
    [SerializeField] Button button;
    public void UnlockLevel()
    {
        button.interactable = true;
    }
}
