using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유니티 어트리뷰트
// 이 스크립트가 추가될 때 typeof() 안에 있는 것도 자동적으로 추가됨
[RequireComponent(typeof(CharState))]
[RequireComponent(typeof(CapsuleCollider))]

public class MyCharController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2f;
    private Vector3 move = Vector3.zero;

    private CharState charState;
    private CapsuleCollider col;


    private void Awake()
    {
        charState = GetComponent<CharState>();
        col = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        UserInput();
        Locomotion();
        CharAnims();

    }



    void UserInput()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.z = Input.GetAxisRaw("Vertical");
        move.Normalize();
    }

    void Locomotion()
    {
        transform.position += Time.deltaTime * moveSpeed * move;
        // 이동하는 방향으로 회전
        transform.LookAt(transform.position + move);
    }

    void CharAnims()
    {

    }
}
