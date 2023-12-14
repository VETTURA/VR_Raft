using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Conductor
{
    public enum Scenes
    {
        MainMenu = 0,
        DemoScene = 1
    }

    public void ShowScene(Scenes scene)
    {
        SceneManager.LoadScene((int)scene, LoadSceneMode.Single);
    }
}
