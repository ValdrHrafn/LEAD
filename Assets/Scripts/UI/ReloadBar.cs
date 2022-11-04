using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadBar : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        
    }

    public void SetChamberingTime(float chamberingTime)
    {
        slider.maxValue = chamberingTime;
        slider.value = 0;
    }
    public void Progress(float reloadProgress)
    {
        slider.value = System.Convert.ToSingle(reloadProgress);
    }
}