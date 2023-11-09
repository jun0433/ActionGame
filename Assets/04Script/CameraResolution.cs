using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private void Awake()
    {
        Camera cam = GetComponent<Camera>();
        // 현재 카메라의 rect값을 가져옴
        Rect rt = cam.rect;
        // 1이상이 나오면 16:9보다 현재 화면이 더 큰 상황
        float scale_Height = ((float)Screen.width / Screen.height) / ((float)16 / 9);
        float scale_Width = 1f / scale_Height;

        if(scale_Height < 1f)// 높이 조정
        {
            rt.height = scale_Height;
            rt.y = (1f - scale_Height) / 2f;
        }
        else // 넓이 조정
        {
            rt.width = scale_Width;
            rt.x = (1f - scale_Width) / 2f;
        }

        cam.rect = rt;
    }
}
