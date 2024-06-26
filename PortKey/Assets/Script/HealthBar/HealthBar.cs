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
    }

    public void UpdateRightPlayerHealthBar(float currentHealth, float maxHealth)
    {
        rightSlider.value = currentHealth / maxHealth;
    }



    // Update is called once per frame
    void Update() { }
}