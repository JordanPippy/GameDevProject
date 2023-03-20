using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI hpText;

    void Start()
    {
        
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        hpText.text = health + "";
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        hpText.text = health + "";
    }
}
