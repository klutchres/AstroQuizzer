using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Sandbox : MonoBehaviour
{
    public Material ModMat;
    public float lerpSpeed = 2;

    float lim, rotation;

    void Update()
    {
        rotation = ModMat.GetFloat("_Rotation");
        lim = rotation < 1 ? 360 : rotation > 359 ? 0 : lim;
        ModMat.SetFloat("_Rotation", Mathf.Lerp(ModMat.GetFloat("_Rotation"), lim, (Time.deltaTime * 0.1f) * lerpSpeed));
    }
}