using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//code from https://www.youtube.com/watch?v=PpIkrff7bKU

public class SceneSwitch : MonoBehaviour
{
    int MAP_SCENE_BUILD_NUMBER = 7;

    public void loaderLvl1()
    {

        GameLevelsManager.Instance.Level = 1; //setting the current level globally that player is on

        SceneManager.LoadScene("Level1");
    }

    public void loaderLvl2()
    {
        GameLevelsManager.Instance.Level = 2; //setting the current level globally that player is on


        SceneManager.LoadScene("Level2");
    }

    public void loaderLvl3()
    {

        GameLevelsManager.Instance.Level = 3; //setting the current level globally that player is on


        SceneManager.LoadScene("Level3");
    }

    public void loaderLvl4()
    {

        GameLevelsManager.Instance.Level = 4; //setting the current level globally that player is on


        SceneManager.LoadScene("Level4");
    }

    public void loaderLvl5()
    {

        GameLevelsManager.Instance.Level = 5; //setting the current level globally that player is on


        SceneManager.LoadScene("Level5");
    }

    public void loaderTut()
    {

        //GameLevelsManager.Instance.Level = 4; //setting the current level globally that player is on


        SceneManager.LoadScene("Tutorial");
    }

    public void loaderTut2()
    {

        //GameLevelsManager.Instance.Level = 4; //setting the current level globally that player is on


        SceneManager.LoadScene("Tutorial2");
    }

    public void loaderTut3()
    {

        //GameLevelsManager.Instance.Level = 4; //setting the current level globally that player is on


        SceneManager.LoadScene("Tutorial3");
    }

    public void loaderTut4()
    {

        //GameLevelsManager.Instance.Level = 4; //setting the current level globally that player is on


        SceneManager.LoadScene("Tutorial4");
    }

    public void loaderMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void loaderStarter()
    {
        SceneManager.LoadScene("Starter");
    }

    public void loadMap()
    {
        SceneManager.LoadScene("Map");
        //SceneManager.LoadScene(MAP_SCENE_BUILD_NUMBER);
    }

}