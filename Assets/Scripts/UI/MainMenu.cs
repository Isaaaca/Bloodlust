using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int tutorialSceneIndex = 1;
    [SerializeField] private GameObject defaultSelection = null;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultSelection);
        SaveManager.Load();

    }
    public void LoadLevel(int sceneIndex)
    {
        SaveManager.playerSpawnPoint = Vector2.zero;
        SaveManager.ClearLevelData("0"+(sceneIndex - 1).ToString());
        SceneManager.LoadScene(sceneIndex);

    }

    public void NewGame()
    {
        SaveManager.Clear();
        SaveManager.Save();
        SceneManager.LoadScene(tutorialSceneIndex);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(SaveManager.currentLevelSceneCode);
        
    }
}
