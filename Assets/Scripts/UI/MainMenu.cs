using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int tutorialSceneIndex = 1;
    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void NewGame()
    {
        SaveManager.Clear();
        SceneManager.LoadScene(tutorialSceneIndex);
    }
}
