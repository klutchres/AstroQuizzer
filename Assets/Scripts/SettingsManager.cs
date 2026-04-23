using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Accessibility;
using System.Runtime.Serialization.Configuration;

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
    public GameObject tFrameRateSetting;

    int vSync = 1, tFrameRate = 1, antialiasing = 2, texLimit = 1, shadows = 1, shadowRes = 2, renderScl = 2;

    void Awake() 
    {
        SettingsManager._instance = this;
        LoadSettings();
    }

    public void UpdateSettings()
    {
        switch (vSync)
        {
            case 0:
                QualitySettings.vSyncCount = 0;
                break;
            case 1:
                QualitySettings.vSyncCount = 1;
                break;
        }

        switch (renderScl)
        {
            case 0:
                UniversalRenderPipeline.asset.renderScale = 0.5f;
                break;
            case 1:
                UniversalRenderPipeline.asset.renderScale = 0.75f;
                break;
            case 2:
                UniversalRenderPipeline.asset.renderScale = 1f;
                break;
            case 3:
                UniversalRenderPipeline.asset.renderScale = 2f;
                break;
        }

        switch (shadows)
        {
            case 0:
                QualitySettings.shadows = UnityEngine.ShadowQuality.Disable;
                break;
            case 1:
                QualitySettings.shadows = UnityEngine.ShadowQuality.All;
                break;
        }

        switch (shadowRes)
        {
            case 0:
                QualitySettings.shadowResolution = UnityEngine.ShadowResolution.Low;
                break;
            case 1:
                QualitySettings.shadowResolution = UnityEngine.ShadowResolution.Medium;
                break;
            case 2:
                QualitySettings.shadowResolution = UnityEngine.ShadowResolution.High;
                break;
            case 3:
                QualitySettings.shadowResolution = UnityEngine.ShadowResolution.VeryHigh;
                break;
        }

        switch (tFrameRate)
        {
            case 0:
                Application.targetFrameRate = 30;
                break;
            case 1:
                Application.targetFrameRate = 60;
                break;
            case 2:
                Application.targetFrameRate = 120;
                break;
            case 3:
                Application.targetFrameRate = -1;
                break;
        }

        //FullRes(HI)-0, HalfRes(ME)-1, QuarterRes(LO)-2
        switch (texLimit)
        {
            case 0:
                QualitySettings.masterTextureLimit = 2;
                break;
            case 1:
                QualitySettings.masterTextureLimit = 1;
                break;
            case 2:
                QualitySettings.masterTextureLimit = 0;
                break;
        }
        
        switch (antialiasing)
        {
            case 0:
                QualitySettings.antiAliasing = 0;
                break;
            case 1:
                QualitySettings.antiAliasing = 2;
                break;
            case 2:
                QualitySettings.antiAliasing = 4;
                break;
            case 3:
                QualitySettings.antiAliasing = 8;
                break;
        }

        tFrameRateSetting.SetActive(vSync == 0);
        SaveSettings();
        Debug.Log(vSync + ", " + tFrameRate + ", " + antialiasing + ", " + texLimit + ", " + shadows + ", " + shadowRes + ", " + renderScl);
    }

    // Load Settings from PlayerPrefs
    public void LoadSettings()
    {
        try
        {
            vSync = PlayerPrefs.GetInt("vSync");
            tFrameRate = PlayerPrefs.GetInt("tFrameRate");
            antialiasing = PlayerPrefs.GetInt("antialiasing");
            texLimit = PlayerPrefs.GetInt("texLimit");
            shadows = PlayerPrefs.GetInt("shadows");
            shadowRes = PlayerPrefs.GetInt("shadowRes");
            renderScl = PlayerPrefs.GetInt("renderScl");
        }
        catch (UnityException e)
        {
            Debug.Log($"<Color=red>{e}</Color>");
        }

        UpdateSettings();
    }

    // Save Settings to PlayerPrefs
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("vSync", vSync);
        PlayerPrefs.SetInt("tFrameRate", tFrameRate);
        PlayerPrefs.SetInt("antialiasing", antialiasing);
        PlayerPrefs.SetInt("texLimit", texLimit);
        PlayerPrefs.SetInt("shadows", shadows);
        PlayerPrefs.SetInt("shadowRes", shadowRes);
        PlayerPrefs.SetInt("renderScl", renderScl);
    }

    public void ModifyVSync(int value)
    {
        vSync = value;
        UpdateSettings();
    }

    public void ModifyTargetFrameRate(int value)
    {
        tFrameRate = value;
        UpdateSettings();
    }

    public void ModifyAntiAliasing(int value)
    {
        antialiasing = value;
        UpdateSettings();
    }

    public void ModifyTextureLimit(int value)
    {
        texLimit = value;
        UpdateSettings();
    }

    public void ModifyShadow(int value)
    {
        shadows = value;
        UpdateSettings();
    }

    public void ModifyShadowResolution(int value)
    {
        shadowRes = value;
        UpdateSettings();
    }

    public void ModifyRenderScale(int value)
    {
        renderScl = value;
        UpdateSettings();
    }
}
