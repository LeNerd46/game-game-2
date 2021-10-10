using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public TMP_InputField saveName;
    public GameObject loadButtonPrefab;
    public Transform savesHolder;

    public PlayerHealth player;
    public GameObject loadMenu;

    public void Save()
    {
        SerializationManager.Save(saveName.text, SaveData.Instance);
    }

    public string[] saveFiles;

    public void GetLoadFiles()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/saves/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/saves/");

        saveFiles = Directory.GetFiles(Application.persistentDataPath + "/saves/");
    }

    public void ShowLoadScreen()
    {
        GetLoadFiles();

        foreach (Transform button in savesHolder)
        {
            if(!button.name.Equals("Load Game Text"))
                Destroy(button.gameObject);
        }

        for (int i = 0; i < saveFiles.Length; i++)
        {
            GameObject buttonObject = Instantiate(loadButtonPrefab);
            buttonObject.transform.SetParent(savesHolder, false);

            var index = i;
            buttonObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                // Where we load the game
                if (GameManager.instance.onGameLoaded != null)
                {
                    SaveData.Instance = (SaveData)SerializationManager.Load(saveFiles[index]);
                    GameManager.instance.onGameLoaded.Invoke();
                }

                loadMenu.SetActive(false);
            });

            buttonObject.GetComponentInChildren<TextMeshProUGUI>().text = saveFiles[index].Replace(Application.persistentDataPath + "/saves/", "");
        }
    }
}