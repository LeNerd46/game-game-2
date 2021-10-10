using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.instance.LoadScene(1, 2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}