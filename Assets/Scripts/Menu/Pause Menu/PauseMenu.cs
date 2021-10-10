using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject UI;
    public GameObject settings;
    public GameObject saveMenu;
    public GameObject loadMenu;

    public TMP_InputField saveName;

    public PlayerHealth player;

    public static bool paused;
    private bool settingsOpen;

    void Update()
    {
        if (InputManager.instance.GetKeyDown(Actions.Pause))
            paused = !paused;

        if (paused && !settingsOpen)
            Pause();
        else if (!paused && !settingsOpen)
            Resume();

        if (settingsOpen)
            Time.timeScale = 0f;
    }

    public void Pause()
    {
        settingsOpen = false;
        paused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        UI.SetActive(false);
        settings.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        settingsOpen = false;
        paused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        UI.SetActive(true);
        settings.SetActive(false);

        if (!Inventory.MenuOpen)
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenSettings()
    {
        settingsOpen = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(false);
        settings.SetActive(true);
    }

    public void Quit()
    {
        GameManager.instance.LoadScene(2, 1);
    }

    public void OpenSaveMenu()
    {
        saveMenu.SetActive(true);
    }

    public void CloseSaveMenu()
    {
        saveMenu.SetActive(false);
    }

    public void SaveGame()
    {
        if (GameManager.instance.onGameSaved != null)
            GameManager.instance.onGameSaved.Invoke();

        SaveManager.instance.Save();

        saveMenu.SetActive(false);
    }

    public void OpenLoadMenu()
    {
        loadMenu.SetActive(true);
        SaveManager.instance.ShowLoadScreen();
    }

    public void CloseLoadMenu()
    {
        loadMenu.SetActive(false);
    }
}
