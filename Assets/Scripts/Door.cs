using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public string id;
    public bool active;
    [Space]
    public float activateDistance = 2f;
    public KeyCode AccessKey = KeyCode.Return;
    [Space]
    public GameObject InfoUI;
    public GameObject StudyUI;
    [Space]
    public Text infoTextDisplay;
    [TextArea] public string infoText;
    [Space]
    public Text titleDisplay;
    public Text contentDisplay;
    [TextArea] public string Content;

    float d;
    void Update()
    {
        active = !SceneManager._instance.idle;
        if (active) d = Vector3.Distance(Pilot._instance.transform.position, transform.position);
        InfoUI.SetActive(d < activateDistance && active && SceneManager._instance.moving);
        StudyUI.SetActive(active && SceneManager._instance.studying && SceneManager._instance.study == id);
        if (d < activateDistance && active)
        {
            infoTextDisplay.text = infoText;
            if (Input.GetKeyDown(AccessKey))
            {
                SceneManager._instance.study = id;
                SceneManager._instance.PlayCutScene(1);
            }
        }
        if (SceneManager._instance.studying && SceneManager._instance.study == id)
        {
            titleDisplay.text = id;
            contentDisplay.text = Content;
        }
    }

    public void Done()
    {
        SceneManager._instance.study = string.Empty;
        SceneManager._instance.PlayCutScene(2);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 0, 0.375f);
        Gizmos.DrawSphere(transform.position, activateDistance);
    }
}
