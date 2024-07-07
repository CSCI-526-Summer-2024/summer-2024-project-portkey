using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PortKey.Assets.Script;

//code from https://www.youtube.com/watch?v=PpIkrff7bKU

public class SceneSwitch : MonoBehaviour
{
    //int MAP_SCENE_BUILD_NUMBER = 7;

    //LEVELS
    public void loaderLvl1()
    {
        if (!TutorialInfo.lvl1) {
            loaderTut();
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
            loaderTut4();
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
        TutorialInfo.lvl1 = true;
    }
    public void loaderTut2()
    {
        SceneManager.LoadScene("Tutorial2");
        TutorialInfo.lvl3 = true;
    }
    public void loaderTut3()
    {
        SceneManager.LoadScene("Tutorial3");
        TutorialInfo.lvl4 = true;
    }
    public void loaderTut4()
    {
        SceneManager.LoadScene("Tutorial4");
        TutorialInfo.lvl5 = true;
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