using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;

    private void Awake()
    {
        target = GameObject.Find("MyCharacter").transform;
    }


    private void Update()
    {
        transform.position = target.position + offset;
    }
}
