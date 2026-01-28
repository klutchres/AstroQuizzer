using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MainHost
{
    public static float f_Random(float min, float max) => UnityEngine.Random.Range(min, max);
    public static int f_Random(int min, int max) => UnityEngine.Random.Range(min, max);
    public static Vector3 f_RandomVector(Vector3 min, Vector3 max) => new Vector3(f_Random(min.x, max.x), f_Random(min.y, max.y), f_Random(min.z, max.z));
    public static Vector3 f_RandomVector(Vector3 min, Vector3 max, float divide) => new Vector3(f_Random(min.x, max.x) / divide, f_Random(min.y, max.y) / divide, f_Random(min.z, max.z) / divide);
    public static float f_BalanceValues(float a_max, float a_value, float a_min, float b_max, float b_min)
    {
        //  This coulf be replaced with:
        //  Mathf.Clamp01(value / limit);
        float a = a_max, b = a_value, c = a_min, d = b_max, e = b_min;
        float A = b - c, B = a - c, C = d - e;
        float sectA = C * A, sectB = B * e;
        float vA = sectA + sectB, finalAnswer = vA / B;
        return finalAnswer;
    }

    public static Vector3 f_BuildVector(float x, float y, float z) => new Vector3(x, y, z);
    public static Vector3 f_BuildVector(float n) => new Vector3(n, n, n);

    public static void f_Log(string msg) => Debug.Log(msg);
}
