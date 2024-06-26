using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private Slider leftSlider;

    [SerializeField] private Slider rightSlider;

    public void UpdateLeftPlayerHealthBar(float currentHealth, float maxHealth)
    {
        leftSlider.value = currentHealth / maxHealth;
        Debug.Log("Left Player Health: " + leftSlider.value + "currentHealth: " + currentHealth + "maxHealth: " + maxHealth);
    }

    public void UpdateRightPlayerHealthBar(float currentHealth, float maxHealth)
    {
        rightSlider.value = currentHealth / maxHealth;
        Debug.Log("Right Player Health: " + rightSlider.value + "currentHealth: " + currentHealth + "maxHealth: " + maxHealth);
    }



    // Update is called once per frame
    void Update() { }
}