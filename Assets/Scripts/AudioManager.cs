using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Vector2 pitchRange;
    [Space]
    public AudioSource clickSource;
    public AudioClip[] clickClips;
    [Space]
    public AudioSource pilotSource;
    public AudioClip[] pilotClips;
    [Space]
    public AudioSource highlightSource;
    public AudioClip[] highlightClips;

    [HideInInspector] public static AudioManager instance;

    void Awake() { AudioManager.instance = this; }
    void Update()
    {

    }

    public void ClickSFX()
    {
        clickSource.clip = clickClips[MainHost.f_Random(0, clickClips.Length)];
        clickSource.pitch = MainHost.f_Random(pitchRange.x, pitchRange.y);
        clickSource.Play();
    }

    public void PilotSFX()
    {
        pilotSource.clip = pilotClips[MainHost.f_Random(0, pilotClips.Length)];
        pilotSource.pitch = MainHost.f_Random(pitchRange.x, pitchRange.y);
        pilotSource.Play();
    }

    public void HighlightSFX()
    {
        highlightSource.pitch = MainHost.f_Random(pitchRange.x, pitchRange.y);
        highlightSource.PlayOneShot(highlightClips[MainHost.f_Random(0, highlightClips.Length)]);
    }

    public void CorrectSFX()
    {

    }
}
