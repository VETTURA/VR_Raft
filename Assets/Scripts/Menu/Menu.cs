using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenu;

    [SerializeField]
    GameObject settingsMenu;

    Conductor conductor = new Conductor();

    #region Main Menu
    public void NewGame()
    {
        conductor.ShowScene(Conductor.Scenes.DemoScene);
    }

    public void ContinueGame()
    {
        conductor.ShowScene(Conductor.Scenes.DemoScene);
    }

    public void Settings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion

    #region Settings

    public void Back()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    #endregion
}