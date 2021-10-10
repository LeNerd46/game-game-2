using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject loadingScreen;

    public Slider progressBar;
    public float totalProgress;

    public delegate void OnEnemyDeathCallback(EnemyProfile enemyProfile);
    public OnEnemyDeathCallback onEnemyDeathCallback;

    public delegate void OnItemPickUp(Item pickedUpItem);
    public OnItemPickUp onItemPickup;

    public delegate void OnLocationEntered(Image locationImage);
    public OnLocationEntered onLocationEntered;

    public delegate void OnGameSaved();
    public OnGameSaved onGameSaved;
    
    public delegate void OnGameLoaded();
    public OnGameLoaded onGameLoaded;

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public TextMeshProUGUI objectiveTextObject;
    public Animator animator;

    void Awake()
    {
        if (instance == null)
            instance = this;

        // SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Loads a scene
    /// </summary>
    /// <param name="unloadSceneIndex">Index of the scene to unload</param>
    /// <param name="loadSceneIndex">Index of the scene to load</param>
    public void LoadScene(int unloadSceneIndex, int loadSceneIndex)
    {
        loadingScreen.SetActive(true);

        scenesLoading.Add(SceneManager.UnloadSceneAsync(unloadSceneIndex));
        scenesLoading.Add(SceneManager.LoadSceneAsync(loadSceneIndex, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public void UpdateTracker(string objectiveText)
    {
        objectiveTextObject.text = objectiveText;
        animator.Play("QuestProgressTextFadeIn");
    }

    public IEnumerator GetSceneLoadProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalProgress += operation.progress;
                }

                totalProgress = (totalProgress / scenesLoading.Count) * 100f;

                progressBar.value = Mathf.RoundToInt(totalProgress);

                yield return null;
            }
        }

        loadingScreen.SetActive(false);
    }
}
