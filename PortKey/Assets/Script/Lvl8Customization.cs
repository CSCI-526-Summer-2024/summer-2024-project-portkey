using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using PortKey.Assets.Script;

public class Lvl8Customization : MonoBehaviour
{
    public Toggle[] options;
    public Button startBtn;
    private SceneSwitch sceneSwitch;

    void Start()
    {
        startBtn.onClick.AddListener(OnSubmit);
        sceneSwitch = FindObjectOfType<SceneSwitch>();
    }

    void OnSubmit()
    {
        Level8Info.scoreUp = false;
        Level8Info.cntrFlip = false;
        Level8Info.lives = false;
        Level8Info.antiHealth = false;
        Level8Info.turtle = false;
        Level8Info.shooting = false;

        if (options.Length > 0 && options[0].isOn)
        {
            Level8Info.scoreUp = true;
        }
        if (options.Length > 1 && options[1].isOn)
        {
            Level8Info.cntrFlip = true;
        }
        if (options.Length > 2 && options[2].isOn)
        {
            Level8Info.lives = true;
        }
        if (options.Length > 3 && options[3].isOn)
        {
            Level8Info.antiHealth = true;
        }
        if (options.Length > 4 && options[4].isOn)
        {
            Level8Info.turtle = true;
        }
        if (options.Length > 5 && options[5].isOn)
        {
            Level8Info.shooting = true;
        }

        sceneSwitch.loaderLvl8();

        //Debug.Log("Level8Info Updated:");
        //Debug.Log("scoreUp: " + Level8Info.scoreUp);
        //Debug.Log("cntrFlip: " + Level8Info.cntrFlip);
        //Debug.Log("lives: " + Level8Info.lives);
        //Debug.Log("antiHealth: " + Level8Info.antiHealth);
        //Debug.Log("turtle: " + Level8Info.turtle);
        //Debug.Log("shooting: " + Level8Info.shooting);
    }
}