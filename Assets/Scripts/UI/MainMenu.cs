using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int tutorialSceneIndex = 1;
    [SerializeField] private GameObject defaultSelection = null;
    [SerializeField] private ScreenFader seceneChangeScreen = null;

    private static bool loaded = false;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultSelection);
        BgmManager.instance.FadeOut(0f);
        if (!loaded)
        {
            SaveManager.Load();
            loaded = true;
        }

    }
    public void LoadLevel(int sceneIndex)
    {
        SaveManager.playerSpawnPoint = Vector2.zero;
        SaveManager.playerLust = 0;
        SaveManager.ClearLevelData("0"+(sceneIndex - 1).ToString());
        StartCoroutine(TransitionToScene(sceneIndex));

    }
    IEnumerator TransitionToScene(int levelIndex)
    {
        seceneChangeScreen.FadeToBlack();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelIndex);

    }

    public void NewGame()
    {
        SaveManager.Clear();
        SaveManager.Save();
        StartCoroutine(TransitionToScene(tutorialSceneIndex));
    }

    public void ContinueGame()
    {
        StartCoroutine(TransitionToScene(SaveManager.currentLevelSceneCode));
        
    }
}
