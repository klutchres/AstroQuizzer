using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class CustomCheckBox : MonoBehaviour, IPointerClickHandler
{
    public bool value;
    public float lerpValue = 1;
    [Space]
    public Image indicator;
    public Color c_Enabled = Color.black, c_Disabled = Color.white;
    Color targetColor;

    void Update()
    {
        indicator.color = Color.Lerp(indicator.color, targetColor, lerpValue * Time.deltaTime);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        value = !value;
        targetColor = value ? c_Enabled : c_Disabled;
    }
}
