using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Fade : MonoBehaviour
{
    public bool faded;
    public float animSpeed = 0.75f;
    Animator anim;
    void Update()
    {
        if (anim == null) anim = GetComponent<Animator>();
        anim.speed = animSpeed;
        anim.Play(faded ? "FadeIn" : "FadeOut");
    }
}
