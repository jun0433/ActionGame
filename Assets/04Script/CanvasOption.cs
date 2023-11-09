using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasOption : MonoBehaviour
{
    private void Awake()
    {
        Canvas canv = GetComponent<Canvas>();
        canv.renderMode = RenderMode.ScreenSpaceCamera;
    }
}
