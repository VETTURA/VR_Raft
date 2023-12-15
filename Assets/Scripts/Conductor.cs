using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Conductor
{
    public enum Scenes
    {
        MainMenu = 0,
        GameScene = 1
    }

    public static void ShowScene(Scenes scene)
    {
        SceneManager.LoadScene((int)scene, LoadSceneMode.Single);
    }
}
