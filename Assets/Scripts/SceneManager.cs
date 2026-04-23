using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Configuration;
using Boo.Lang;
using UnityEditor;
using UnityEngine.Rendering;

public class SceneManager : MonoBehaviour
{
    public bool flying;
    public bool studying;
    public bool cutscene;
    public bool idle = true;
    public bool moving;
    [Space]
    public Transform camPos;
    public float controlFOV = 60;
    public float activateFlightThreshold = 4;
    public PlayableDirector Director;
    public PlayableAsset[] CutScenes;
    [Space]
    public bool restartOnClick;
    public Vector2 buttonEffectDistance;
    public bool FadeUI;
    public float fadeLerpSpeed = 1;
    public Text[] fadeTargets;
    public Transform pilotHeadPos;
    public Door[] Doors;
    [Space]
    public float posAmp = 0.5f;
    public float posFre = 0.2f, rotAmp = 2f, rotFre = 0.2f;
    [Space]
    public GameObject[] FlightObjects;
    public GameObject[] LandObjects;
    [Space]
    public AudioLowPassFilter musicSource;
    public float cutOffLerpSpeed = 1;
    public float cutOffAmount_Lobby = 100, cutOffAmount_Game = 650;
    Vector3 camStartPos, camStartRot;

    Camera cam;
    [HideInInspector] public static SceneManager _instance;
    [HideInInspector] public string study;
    void Awake()
    {
        if (!SceneManager._instance) SceneManager._instance = this;
        cam = Camera.main;
        camStartPos = camPos.localPosition;
        camStartRot = camPos.localEulerAngles;
    }

    void Start()
    {
           
    }

    float targetCutoff;
    float targetVolume;
    float a;
    void Update()
    {
        if (cam && !flying) cam.fieldOfView = controlFOV;
        if (restartOnClick && Input.GetKeyDown(KeyCode.Tab)) RestartGame();
        a = Mathf.Lerp(a, FadeUI ? 0 : 1, Time.deltaTime * fadeLerpSpeed);
        for (int i = 0; i < fadeTargets.Length; i++)
        {
            Color newColor = new Color(fadeTargets[i].color.r, fadeTargets[i].color.g, fadeTargets[i].color.b, a);
            fadeTargets[i].color = newColor;
        }
        camPos.GetComponent<FreeCamera>().active = moving;
        //Pilot._instance.gameObject.SetActive(!flying);
        if (Vector3.Distance(Pilot._instance.transform.position, Spaceship._instance.transform.position) < activateFlightThreshold && !cutscene && !flying)
        {
            PlayCutScene(3);
            Pilot._instance.gameObject.SetActive(false);
        }

        // Sound Track
        targetVolume = Mathf.Lerp(targetVolume, !flying ? 0.5f : (Spaceship._instance.GetPause() || Spaceship._instance.m_holdTime) ? 0.1f : 0.375f, Time.deltaTime * 3.5f);
        targetCutoff = Mathf.Lerp(targetCutoff, !flying ? cutOffAmount_Lobby : (Spaceship._instance.GetPause() || Spaceship._instance.m_holdTime) ? cutOffAmount_Game : 22000, Time.deltaTime * cutOffLerpSpeed);
        musicSource.GetComponent<AudioSource>().volume = targetVolume;
        musicSource.cutoffFrequency = targetCutoff;

        if (!flying)
        {
            if (idle || studying)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            if (moving || cutscene)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        if (idle)
        {
            foreach (var v in Doors) { v.active = true; }
            float oX = Mathf.Sin(Time.time * posFre) * posAmp;
            float oY = Mathf.Cos(Time.time * posFre * 0.7f) * posAmp * 0.5f;
            camPos.localPosition = camStartPos + MainHost.f_BuildVector(oX, oY, 0);

            float rZ = Mathf.Sin(Time.time * rotFre) * rotAmp;
            camPos.localEulerAngles = camStartRot + MainHost.f_BuildVector(0, 0, rZ);
        }

        if (moving)
        {
            camPos.position = pilotHeadPos.position;
        }

        foreach (var obj in LandObjects) obj.SetActive(!flying);
        foreach (var obj in FlightObjects) obj.SetActive(flying);
    }

    public void PlayCutScene(int position)
    {
        cutscene = true;
        Director.playableAsset = CutScenes[position];
        Director.Play();
    }

    public void QuitGame() => Application.Quit();
    public void RestartGame() => UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
}