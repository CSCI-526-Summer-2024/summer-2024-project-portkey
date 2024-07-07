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
            TutorialInfo.lastScene = 1;
            GameLevelsManager.Instance.Level = 1; //setting the current level globally that player is on
            SceneManager.LoadScene("Level1");
        }
    }
    public void loaderLvl2()
    {
        TutorialInfo.lastScene = 2;
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
            TutorialInfo.lastScene = 3;
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
            TutorialInfo.lastScene = 4;
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
            TutorialInfo.lastScene = 5;
            GameLevelsManager.Instance.Level = 5; //setting the current level globally that player is on
            SceneManager.LoadScene("Level5");
        }
    }

    //TUTORIALS
    public void loaderTut()
    {
        TutorialInfo.lastScene = -1;
        SceneManager.LoadScene("Tutorial");
        TutorialInfo.lvl1 = true;
    }
    public void loaderTut2()
    {
        TutorialInfo.lastScene = -2;
        SceneManager.LoadScene("Tutorial2");
        TutorialInfo.lvl3 = true;
    }
    public void loaderTut3()
    {
        TutorialInfo.lastScene = -3;
        SceneManager.LoadScene("Tutorial3");
        TutorialInfo.lvl4 = true;
    }
    public void loaderTut4()
    {
        TutorialInfo.lastScene = -4;
        SceneManager.LoadScene("Tutorial4");
        TutorialInfo.lvl5 = true;
    }

    //MENUS
    public void loaderMenu()
    {
        TutorialInfo.lastScene = 100;
        SceneManager.LoadScene("Menu");
    }
    public void loaderMenuTut()
    {
        TutorialInfo.lastScene = 100;
        SceneManager.LoadScene("TutorialMenu");
    }
    public void loaderStarter()
    {
        TutorialInfo.lastScene = 100;
        SceneManager.LoadScene("Starter");
    }
    public void loadMap()
    {
        SceneManager.LoadScene("Map");
    }

    //BACK BUTTON
    public void BackButton()
    {
        switch (TutorialInfo.lastScene)
        {
            case 1:
                loaderLvl1();
                break;
            case 2:
                loaderLvl2();
                break;
            case 3:
                loaderLvl3();
                break;
            case 4:
                loaderLvl4();
                break;
            case 5:
                loaderLvl5();
                break;
            case -1:
                loaderTut();
                break;
            case -2:
                loaderTut2();
                break;
            case -3:
                loaderTut3();
                break;
            case -4:
                loaderTut4();
                break;
            case 100:
            default:
                loaderMenu();
                break;
        }
    }

}