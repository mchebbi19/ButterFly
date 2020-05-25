using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader 
{

    public  enum Scene
    {
        GameScene,
        Loading,
        Menu,
    }

    private static Scene targetScene;

    // Start is called before the first frame update
    public static void Load(Scene scene)
    {
       SceneManager.LoadScene(Scene.Loading.ToString());
        targetScene = scene;
       
    }

    public static void LoadtargetScene()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
