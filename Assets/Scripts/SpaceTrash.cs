using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum trashType { NORMAL, BAD }

public class SpaceTrash : MonoBehaviour
{
    public bool randomizeType;
    public trashType type = trashType.NORMAL;
    [Space]
    public bool randomizeSize = true;
    public Vector3 Min, Max;
    [Space]
    public bool useInitVelocity = true;
    public bool randomizeInitVelocity = true;
    public float randomVelocityMax = 1;
    public Vector3 initVelocity;
    [Space]
    public bool changeAppearance = true;
    public Material[] Colors;
    public Renderer[] Renderers;

    Rigidbody rb;
    void Start()
    {
        if (Renderers == null) Renderers = new Renderer[] { GetComponent<Renderer>() };
        rb = GetComponent<Rigidbody>();
        Refresh();
    }
    public void Refresh()
    {
        if (randomizeType) type = MainHost.f_Random(-1, 1) > 0 ? trashType.NORMAL : trashType.BAD;
        if (randomizeSize) this.transform.localScale = MainHost.f_RandomVector(Min, Max);
        if (changeAppearance) foreach (var r in Renderers) r.material = Colors[type == trashType.BAD ? 0 : MainHost.f_Random(1, Colors.Length)]; if (rb && useInitVelocity)
        {
            if (randomizeInitVelocity) initVelocity = MainHost.f_BuildVector(MainHost.f_Random(-randomVelocityMax, randomVelocityMax), MainHost.f_Random(-randomVelocityMax, randomVelocityMax), MainHost.f_Random(-randomVelocityMax, randomVelocityMax));
            rb.velocity = initVelocity;
        }
    }
}
