using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PortKey.Assets.Script;

//code from https://www.youtube.com/watch?v=PpIkrff7bKU

public class SceneSwitch : MonoBehaviour
{
    int MAP_SCENE_BUILD_NUMBER = 7;

    //LEVELS
    public void loaderLvl1()
    {
        if (!TutorialInfo.lvl1) {
            loaderTut();
            TutorialInfo.lvl1 = true;
        } else
        {
            GameLevelsManager.Instance.Level = 1; //setting the current level globally that player is on
            SceneManager.LoadScene("Level1");
        }
    }
    public void loaderLvl2()
    {
        GameLevelsManager.Instance.Level = 2; //setting the current level globally that player is on
        SceneManager.LoadScene("Level2");
    }
    public void loaderLvl3()
    {
        if (!TutorialInfo.lvl3)
        {
            loaderTut2();
            TutorialInfo.lvl3 = true;
        } else
        {
            GameLevelsManager.Instance.Level = 3; //setting the current level globally that player is on
            SceneManager.LoadScene("Level3");
        }
    }
    public void loaderLvl4()
    {
        if (!TutorialInfo.lvl4)
        {
            loaderTut3();
            TutorialInfo.lvl4 = true;
        }
        else
        {
            GameLevelsManager.Instance.Level = 4; //setting the current level globally that player is on
            SceneManager.LoadScene("Level4");
        }
    }
    public void loaderLvl5()
    {
        if (!TutorialInfo.lvl5)
        {
            loaderTut3();
            TutorialInfo.lvl5 = true;
        }
        else
        {
            GameLevelsManager.Instance.Level = 5; //setting the current level globally that player is on
            SceneManager.LoadScene("Level5");
        }
    }

    //TUTORIALS
    public void loaderTut()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void loaderTut2()
    {
        SceneManager.LoadScene("Tutorial2");
    }
    public void loaderTut3()
    {
        SceneManager.LoadScene("Tutorial3");
    }
    public void loaderTut4()
    {
        SceneManager.LoadScene("Tutorial4");
    }

    //MENUS
    public void loaderMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void loaderMenuTut()
    {
        SceneManager.LoadScene("TutorialMenu");
    }
    public void loaderStarter()
    {
        SceneManager.LoadScene("Starter");
    }
    public void loadMap()
    {
        SceneManager.LoadScene("Map");
    }

}