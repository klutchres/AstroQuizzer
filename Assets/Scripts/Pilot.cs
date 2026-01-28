using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class Pilot : MonoBehaviour
{
    public bool active;
    public Transform camPos;
    [Space]
    public GameObject InfoUI;
    public Transform mousePointer;
    public float pointerVisibleThreshold = 1;

    NavMeshAgent Agent;
    [HideInInspector] public static Pilot _instance;
    Camera cam;
    bool moving;
    void Start()
    {
        Pilot._instance = this;
        Agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }
    void Update()
    {
        active = SceneManager._instance.moving;
        if (active)
        {
            Agent.updateRotation = true;
            InfoUI.SetActive(!moving);
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Agent.SetDestination(hit.point);
                    mousePointer.position = hit.point;
                    mousePointer.gameObject.SetActive(true);
                    moving = true;
                    Agent.isStopped = false;
                    AudioManager.instance.PilotSFX();
                }
                else
                {
                    Agent.isStopped = true;
                    moving = false;
                    mousePointer.gameObject.SetActive(false);
                }
            }
            if (Input.GetMouseButtonDown(1) && moving)
            {
                Agent.isStopped = true;
                moving = false;
            }
            if (moving)
            {
                if (Vector3.Distance(mousePointer.position, transform.position) < pointerVisibleThreshold)
                {
                    mousePointer.gameObject.SetActive(false);
                    moving = false;
                }
            }
            else
            {
                mousePointer.gameObject.SetActive(false);
            }
        }
        else
        {
            InfoUI.SetActive(false);
            mousePointer.gameObject.SetActive(false);
            Agent.isStopped = true;
            moving = false;
        }
    }
}
