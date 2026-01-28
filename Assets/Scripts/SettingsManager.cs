using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public enum setting
{
    VSYNC,
    TARGET_FRAME_RATE,
    ANTI_ALIASING,
    TEXTURE_LIMIT,
    SHADOWS,
    SHADOW_RESOLUTION,
    RENDER_SCALE
}
public class SettingsManager : MonoBehaviour
{
    [HideInInspector] public static SettingsManager _instance;
    void Awake() => SettingsManager._instance = this;
    public void ApplySetting(setting target, string value, string title) // Toggle
    {
        switch (target)
        {
            case setting.VSYNC:
                if (value == "ON") QualitySettings.vSyncCount = 1;
                if (value == "OFF") QualitySettings.vSyncCount = 0;
                break;
            case setting.RENDER_SCALE:
                if (value == "0.5x") UniversalRenderPipeline.asset.renderScale = 0.5f;
                if (value == "0.75x") UniversalRenderPipeline.asset.renderScale = 0.75f;
                if (value == "1x") UniversalRenderPipeline.asset.renderScale = 1;
                if (value == "2x") UniversalRenderPipeline.asset.renderScale = 2;
                break;
            case setting.SHADOWS:
                QualitySettings.shadows = value == "OFF" ? UnityEngine.ShadowQuality.Disable : UnityEngine.ShadowQuality.All;
                break;
            case setting.SHADOW_RESOLUTION:
                if (value == "Low") QualitySettings.shadowResolution = UnityEngine.ShadowResolution.Low;
                if (value == "Medium") QualitySettings.shadowResolution = UnityEngine.ShadowResolution.Medium;
                if (value == "High") QualitySettings.shadowResolution = UnityEngine.ShadowResolution.High;
                if (value == "VeryHigh") QualitySettings.shadowResolution = UnityEngine.ShadowResolution.VeryHigh;
                break;
            case setting.TARGET_FRAME_RATE:
                if (value == "30") Application.targetFrameRate = 30;
                if (value == "60") Application.targetFrameRate = 60;
                if (value == "120") Application.targetFrameRate = 120;
                if (value == "Unlimited") Application.targetFrameRate = -1;
                break;
            case setting.TEXTURE_LIMIT:
                //FullRes(HI)-0, HalfRes(ME)-1, QuarterRes(LO)-2
                if (value == "High") QualitySettings.masterTextureLimit = 0;
                if (value == "Medium") QualitySettings.masterTextureLimit = 1;
                if (value == "Low") QualitySettings.masterTextureLimit = 2;
                break;
            case setting.ANTI_ALIASING:
                //OFF-0 2, 4, 8;
                if (value == "OFF") QualitySettings.antiAliasing = 0;
                if (value == "2x") QualitySettings.antiAliasing = 2;
                if (value == "4x") QualitySettings.antiAliasing = 4;
                if (value == "8x") QualitySettings.antiAliasing = 8;
                break;
        }
        PlayerPrefs.SetString(title, value);
        Debug.Log("infunc: " + title + " -> " + PlayerPrefs.GetString(title));
    }
}
