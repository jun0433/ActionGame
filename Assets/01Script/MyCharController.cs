using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����Ƽ ��Ʈ����Ʈ
// �� ��ũ��Ʈ�� �߰��� �� typeof() �ȿ� �ִ� �͵� �ڵ������� �߰���
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
        // �̵��ϴ� �������� ȸ��
        transform.LookAt(transform.position + move);
    }

    void CharAnims()
    {

    }
}
