using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityScript.Scripting.Pipeline;

public class CustomToggle : MonoBehaviour
{
    public setting targetSetting;
    public string Title;
    [Space]
    public Text titleDisplay;
    public Text valueDisplay;
    [Space]
    public string[] values;

    public RectTransform titleTransform, buttonTransform;
    RectTransform thisTransform;

    int value;

    void Awake() { thisTransform = GetComponent<RectTransform>(); LoadSettings(); }
    void Update()
    {
        titleDisplay.text = Title;
        valueDisplay.text = values[value]; 
        titleTransform.sizeDelta = new Vector2(thisTransform.sizeDelta.x * 0.8f, titleTransform.sizeDelta.y);
        buttonTransform.sizeDelta = new Vector2(thisTransform.sizeDelta.x * 0.2f, buttonTransform.sizeDelta.y);
    }
    public void Next()
    {
        value = value == values.Length - 1 ? 0 : value + 1;
        SettingsManager._instance.ApplySetting(targetSetting, values[value], Title);
        AudioManager.instance.ClickSFX();
    }

    void LoadSettings()
    {
        if (PlayerPrefs.GetString(Title) != null)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == PlayerPrefs.GetString(Title)) { value = i;  Debug.Log($"Target: {values[i]}"); break; }
            }
            SettingsManager._instance.ApplySetting(targetSetting, values[value], Title);
            Debug.Log(PlayerPrefs.GetString(Title));
        }
    }
}
