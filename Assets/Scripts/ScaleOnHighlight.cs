using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ScaleOnHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float lerpSpeed = 1.5f;
    public Vector3 scaleVec;
    Outline outline;
    Vector3 targetVec;
    RectTransform t;
    void Start() { outline = GetComponent<Outline>(); t = GetComponent<RectTransform>(); targetVec = Vector3.one; }

    void Update()
    {
        t.localScale = Vector3.Lerp(t.localScale, targetVec, lerpSpeed * Time.deltaTime);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (outline) outline.effectDistance = SceneManager._instance.buttonEffectDistance;
        AudioManager.instance.HighlightSFX();
        targetVec = scaleVec;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (outline) outline.effectDistance = Vector2.zero;
        targetVec = Vector3.one;
    }
}
