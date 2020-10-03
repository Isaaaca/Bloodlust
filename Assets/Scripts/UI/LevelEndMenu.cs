using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelEndMenu : MonoBehaviour
{
    [SerializeField] private ScreenFader screen = null;
    [SerializeField] private GameObject panel = null;
    [SerializeField] private GameObject defaultSelection = null;

    public void ShowMenu()
    {
        panel.SetActive(true);
        screen.FadeIn();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultSelection);

    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
