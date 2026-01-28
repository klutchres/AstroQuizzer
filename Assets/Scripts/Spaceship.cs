using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class Spaceship : MonoBehaviour
{
    [Header("CENTER MODULE")]
    public bool intialised;
    public ParentConstraint camLockPar;
    public RotationConstraint camLockRot;
    [Space]
    public float inputLagCompensator = 1;
    public float xLimit = 20;
    public float xMoveSpeed = 20;
    [Space]
    public float rollSpeed = 5f;
    public float maxRoll = 30f;
    [Space]
    public Transform camPivotPoint;
    public float camLerpSpeed;
    public Transform camPivot;
    public Camera cam;
    public float defaultFOV = 70;
    public float movingFOV = 80;
    public float boostFOV = 10;
    public float heldFOV = 50;
    [Space]
    public float amplitude = 0.05f;
    public float frequency = 20, swayFrequency = 2;
    [Space]
    public float acceleration = 1;
    public float maxSpeed = 30;
    public float boostMaxSpeed = 60;
    [Space]
    public float boostPower = 300;
    public float boostDuration = 1;
    public float boostCoolDown = 3;
    [Space]
    public AudioSource EngineSource;
    public AudioClip engineClip;
    public float pitchMin = 0.5f, pitchMax = 1.25f;
    public float speedToPitchRatio = 0.05f;
    public float pitchLerpSpeed = 2;
    [Space]
    public AudioSource BoostSource;
    public AudioClip boostClip;
    [Space]
    public AudioSource HitSource;
    public AudioClip[] hitClips;
    [Space]
    public AudioSource[] musicSources;
    public AudioClip[] musicClips;
    [Space]
    public GameObject fuelUI;
    public Image fuelBar;
    public float fuelBarLerp = 2;
    public Color NormalColor = Color.white, BoostColor = Color.cyan, LowColor = Color.red;
    [Space]
    public float startingFuel = 350f;
    public float maxFuel = 1000f;
    public float fuelIncrement = 200f;
    public float fuelConsumptionSpeed = 10f;
    public float fuelBoostConsumptionMultiplier = 3;
    public float lowFuelThreshold = 100f;
    public GameObject FailedUI, LowFuelUI;
    [Space]
    public Vector2 questionIntervalRange;
    public GameObject PauseMenuUI;
    [Space]
    public float maxZ = 250;
    public float resetZ = -20;
    [Space]
    public float holdTimeScale = 0.2f;
    public float timeLerp = 5;
    [Space]
    public KeyCode BoostKey = KeyCode.Space;
    public KeyCode FireKey = KeyCode.Mouse0;
    public KeyCode HoldKey = KeyCode.Mouse1;
    public KeyCode PauseKey = KeyCode.Escape;
    [Space]
    public bool drawGizmos;
    public Color gizmosColor = Color.red;
    [Space]
    public bool modifyVelocity;
    public Vector3 modV;

    [HideInInspector] public static Spaceship _instance;
    [HideInInspector] public bool m_holdTime;
    Rigidbody rb;
    float targetFOV = 60, speed, targetScale = 1.0f, currentFuel, qT;

    List<AudioSource> msources;
    bool m_moving, m_boost;
    bool paused;
    bool failed;

    /// <summary>
    /// bT - Boost Timer
    /// bCT - Boost Countdown Timer
    /// </summary>
    float bT, bCT;

    Vector3 initCam;
    void Awake()
    {
        Spaceship._instance = this;
        initCam = cam.transform.localPosition;
        rb = GetComponent<Rigidbody>();
        targetFOV = defaultFOV;
        bT = boostDuration;
        currentFuel = startingFuel;
        qT = MainHost.f_Random(questionIntervalRange.x, questionIntervalRange.y);

        msources = new List<AudioSource>();
        for (int i = 0; i < musicClips.Length; i++)
        {
            musicSources[i].clip = musicClips[i];
            msources.Add(musicSources[i]);
        }
    }
    float h;
    void FixedUpdate()
    {
        //Movememnt
        if (intialised)
        {
            if (m_boost) { if (speed <= boostMaxSpeed) rb.AddForce(Vector3.forward * boostPower); }
            if (m_moving && speed < maxSpeed) { rb.velocity += (Vector3.forward * (acceleration / 10)); }
            var newH = (h > 0 ? (transform.position.x < xLimit ? h : 0) : h < 0 ? (transform.position.x > -xLimit ? h : 0) : h);
            if (h != 0) transform.position += MainHost.f_BuildVector((Mathf.Clamp01(speed / maxSpeed) * 2 + 0.1f) * newH * (xMoveSpeed / 100), 0, 0);
            if (currentFuel <= 1)
            {
                failed = true;
            }
            qT = uestionManager.instance.open ? qT = MainHost.f_Random(questionIntervalRange.x, questionIntervalRange.y) : qT;
            qT -= Time.deltaTime;
            if (qT <= 0)
            {
                uestionManager.instance.Open();
            }
            rb.velocity = MainHost.f_BuildVector((transform.position.x > xLimit || transform.position.x < -xLimit) ? 0 : rb.velocity.x, 0, rb.velocity.z);
            transform.position = MainHost.f_BuildVector(transform.position.x > xLimit ? xLimit - 0.1f : (transform.position.x < -xLimit ? -xLimit + 0.1f : transform.position.x), transform.position.y, transform.position.z);
            fuelBar.fillAmount = Mathf.Lerp(fuelBar.fillAmount, (currentFuel / maxFuel), fuelBarLerp * Time.deltaTime);
            fuelBar.color = Color.Lerp(fuelBar.color, (currentFuel > lowFuelThreshold ? m_boost ? BoostColor : NormalColor : LowColor), Time.deltaTime * fuelBarLerp);
        }
        fuelUI.SetActive(!paused && !failed && intialised && !m_holdTime);
        LowFuelUI.SetActive(!paused && currentFuel <= lowFuelThreshold && !failed && !m_holdTime);
        FailedUI.SetActive(failed && !paused);
        SceneManager._instance.restartOnClick = failed;
        if (modifyVelocity) rb.velocity = modV;
        camLockPar.constraintActive = !intialised;
        camLockRot.constraintActive = intialised;
    }
    float tRef;
    void Update()
    {
        intialised = !failed && SceneManager._instance.flying;
        if (intialised)
        {
            //Speed Data
            var speedValue = Mathf.Clamp01(speed / maxSpeed);
            speed = rb.velocity.magnitude;

            //Camera's Position & Movement
            if (camPivot & cam)
            {
                camPivot.position = Vector3.Lerp(camPivot.position, camPivotPoint.position, Time.deltaTime * camLerpSpeed);
            }

            //Camera's Field Of View
            targetFOV = !m_holdTime ? (MainHost.f_BalanceValues(maxSpeed, speed, 0, movingFOV, defaultFOV)) + (m_boost ? (boostFOV) : 0) : heldFOV;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * camLerpSpeed * (m_holdTime ? 3f : (m_boost ? 0.25f : 1)));

            //Resetting Spaceship's Position
            if (transform.position.z > maxZ) { transform.position = MainHost.f_BuildVector(transform.position.x, transform.position.y, resetZ); SpawnManager.instance.NewWave(); }

            //Camera Shake
            float shakeAmount = (m_boost ? 2 : 1) * amplitude * speedValue;
            float x = Mathf.Sin(Time.time * frequency) * shakeAmount;
            float y = Mathf.Sin(Time.time * frequency * 1.2f) * shakeAmount;
            float swayX = Mathf.Sin(Time.time * swayFrequency) * shakeAmount * 0.5f;
            float swayY = Mathf.Sin(Time.time * swayFrequency * 1.5f) * shakeAmount * 0.5f;
            Vector3 shakeOffset = MainHost.f_BuildVector(x + swayX, y + swayY, 0);
            cam.transform.localPosition = initCam + shakeOffset;

            //Roll Tilting
            h = Mathf.Lerp(h, Input.GetAxis("Horizontal"), Time.deltaTime * inputLagCompensator);
            float targetZ = -h * maxRoll;
            Vector3 currentRot = transform.localEulerAngles;
            #region Handle Negative Angles
            if (currentRot.z > 180) currentRot.z -= 360;
            #endregion
            float nZ = Mathf.Lerp(currentRot.z, transform.position.x < xLimit && transform.position.x > -xLimit ? targetZ : 0, Time.deltaTime * rollSpeed * speedValue + 0.25f);
            transform.localEulerAngles = MainHost.f_BuildVector(0, 0, nZ);

            //State Management
            m_boost = !m_holdTime && Input.GetKey(BoostKey);
            m_moving = !m_holdTime;

            //Boost
            if (!m_holdTime && m_boost && bCT <= 0)
            {
                bT -= Time.deltaTime;
                if (bT <= 0)
                {
                    bCT = boostCoolDown;
                    bT = boostDuration;
                }
            }
            else bCT -= Time.deltaTime;

            //Time Scale
            targetScale = failed ? 0 : paused ? 0 : !m_holdTime ? 1.0f : holdTimeScale;
            Time.timeScale = Mathf.SmoothDamp(Time.timeScale, targetScale, ref tRef, timeLerp * Time.deltaTime);

            //Cursor
            Cursor.visible = m_holdTime || paused;
            Cursor.lockState = m_holdTime || paused ? CursorLockMode.None : CursorLockMode.Locked;

            //PauseMenu
            if (Input.GetKeyDown(PauseKey) && !failed) paused = (paused == true ? false : true);
            PauseMenuUI.SetActive(paused);

            //Fuel
            currentFuel -= Time.deltaTime * fuelConsumptionSpeed * (m_boost ? fuelBoostConsumptionMultiplier : 1);

            //Audio
            if (!EngineSource.isPlaying) { EngineSource.playOnAwake = true; if(!EngineSource.isPlaying) EngineSource.Play(); EngineSource.loop = true; }
            float targetPitch = MainHost.f_BalanceValues(maxSpeed, speed, 0, pitchMax, pitchMin) + (m_boost ? 0.2f : 0);
            EngineSource.pitch = Mathf.Lerp(EngineSource.pitch, targetPitch, Time.deltaTime * pitchLerpSpeed);
            BoostSource.clip = boostClip; if (!BoostSource.isPlaying) { BoostSource.Play(); } BoostSource.loop = true;
            BoostSource.volume = Mathf.Lerp(BoostSource.volume, m_boost ? 1f : 0, Time.deltaTime * (m_boost ? 5 : 1)); 
            foreach(var s in msources) { s.volume = Mathf.Lerp(s.volume, (!paused && !m_holdTime) ? 0.375f : 0.1f, Time.deltaTime * 3.5f); if (!s.isPlaying) s.Play(); s.loop = true; }
        }
    }
    void OnDrawGizmos() { if (drawGizmos) Gizmos.color = Color.red; Gizmos.DrawLine(Vector3.forward * -maxZ, Vector3.forward * maxZ); }
    public void IncrementFuel() => currentFuel += fuelIncrement;
    public void Resume() => paused = false;

    public bool GetPause() => paused;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<SpaceTrash>())
        {
            HitSource.PlayOneShot(hitClips[MainHost.f_Random(0, hitClips.Length)]);
            currentFuel -= 50;
        }
    }
}