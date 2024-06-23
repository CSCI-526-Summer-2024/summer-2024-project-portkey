using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//code from https://www.youtube.com/watch?v=PpIkrff7bKU

public class SceneSwitch : MonoBehaviour
{
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

    public void loaderMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}