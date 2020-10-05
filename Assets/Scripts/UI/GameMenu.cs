using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private ScreenFader screen = null;
    [SerializeField] private GameObject levelEndPanel = null;
    [SerializeField] private GameObject levelEndDefaultSelection = null;
    [SerializeField] private GameObject pausePanel = null;
    [SerializeField] private GameObject pauseDefaultSelection = null;
    [SerializeField] private GameManager gameManager = null;

    private void Update()
    {
        if(levelEndPanel.activeSelf == false
            && Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (pausePanel.activeSelf) Unpause();
            else Pause();
        }
    }
    public void ShowLevelEndMenu()
    {
        levelEndPanel.SetActive(true);
        screen.FadeIn();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(levelEndDefaultSelection);

    }
    public void Pause()
    {
        gameManager.SetGamePause(true);
        pausePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseDefaultSelection);
    }

    public void Unpause()
    {
        gameManager.SetGamePause(false);
        EventSystem.current.SetSelectedGameObject(null);
        pausePanel.SetActive(false);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    } 
    
    public void MainMenu()
    {
        if (pausePanel.activeSelf) gameManager.SetGamePause(false);
        SceneManager.LoadScene(0);
    }
}
