using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenu;

    [SerializeField]
    GameObject settingsMenu;

    #region Main Menu
    public void NewGame()
    {
        Conductor.ShowScene(Conductor.Scenes.GameScene);
    }

    public void ContinueGame()
    {
        Conductor.ShowScene(Conductor.Scenes.GameScene);
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