using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private void Awake()
    {
        Camera cam = GetComponent<Camera>();
        // ���� ī�޶��� rect���� ������
        Rect rt = cam.rect;
        // 1�̻��� ������ 16:9���� ���� ȭ���� �� ū ��Ȳ
        float scale_Height = ((float)Screen.width / Screen.height) / ((float)16 / 9);
        float scale_Width = 1f / scale_Height;

        if(scale_Height < 1f)// ���� ����
        {
            rt.height = scale_Height;
            rt.y = (1f - scale_Height) / 2f;
        }
        else // ���� ����
        {
            rt.width = scale_Width;
            rt.x = (1f - scale_Width) / 2f;
        }

        cam.rect = rt;
    }
}
