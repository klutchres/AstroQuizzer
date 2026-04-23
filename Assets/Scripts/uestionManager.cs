using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class uestionManager : MonoBehaviour
{
    public bool active;
    public bool open;
    [Space]
    public LectureData[] Questions;
    LectureData current;
    [Space]
    public Text QuestionDisplay;
    public Text opt1, opt2, opt3, opt4;
    [Space]
    public GameObject QuestionUI;
    public AudioSource source;
    public AudioClip correctClip;
    public AudioClip wrongClip;

    [HideInInspector] public static uestionManager instance;
    void Awake()
    {
        uestionManager.instance = this;
    }

    void Fail()
    {
        //Play SFX
        source.clip = wrongClip; source.Play();

        open = false;
    }

    void Pass()
    {
        // Play SFX
        source.clip = correctClip; source.Play();

        open = false;
        Spaceship._instance.IncrementFuel();
    }

    void Update()
    {
        active = SceneManager._instance.flying;
        QuestionUI.SetActive(open && active && !Spaceship._instance.GetPause());
        if (open && active)
        {
            QuestionDisplay.text = current.Question;
            opt1.text = current.Answers[0];
            opt2.text = current.Answers[1];
            opt3.text = current.Answers[2];
            opt4.text = current.Answers[3];
        }
        Spaceship._instance.m_holdTime = (active && open);
    }

    public void SubmitAnswer(int value)
    {
        if (current.Correct == value)
        {
            Pass();
        }
        else
        {
            Fail();
        }
    }

    public void Open()
    {
        current = Questions[MainHost.f_Random(0, Questions.Length)];
        open = true;
    }
}