using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
    public Image[] filledHeartsLeft;
    public Image[] filledHeartsRight;
    

    int currentLivesLeft = 3;
    int currentLivesRight = 3;

    public Vector2 initialPosition = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHeartsVisualLeft()
    {
        for (int i = 0; i < currentLivesLeft; i++)
        {
            filledHeartsLeft[i].transform.localScale = Vector3.one;
        }
        for (int i = currentLivesLeft; i < 3; i++)
        {
            filledHeartsLeft[i].transform.localScale = Vector3.zero;
        }
    }

    void UpdateHeartsVisualRight()
    {
        for (int i = 0; i < currentLivesRight; i++)
        {
            filledHeartsRight[i].transform.localScale = Vector3.one;
        }
        for (int i = currentLivesRight; i < 3; i++)
        {
            filledHeartsRight[i].transform.localScale = Vector3.zero;
        }
    }

    public void IncrementLivesLeft()
    {
        if (currentLivesLeft != 3)
        {
            currentLivesLeft += 1;
            UpdateHeartsVisualLeft();
        }
       
    }

    public void DecrementLivesLeft()
    {
        currentLivesLeft -= 1;
        UpdateHeartsVisualLeft();
    }

    public void IncrementLivesRight()
    {
        if (currentLivesRight != 3)
        {
            currentLivesRight += 1;
            UpdateHeartsVisualRight();
        }
    }

    public void DecrementLivesRight()
    {
        currentLivesRight -= 1;
        UpdateHeartsVisualRight();
    }

    public int GetLivesRight()
    {
        return currentLivesRight;
    }

    public int GetLivesLeft()
    {
        return currentLivesLeft;
    }
}
