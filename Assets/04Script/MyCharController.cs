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
    private Animator anim;

    private bool input_Attack01;
    private bool input_Attack02;

    private int aniHash_Attack01 = Animator.StringToHash("doAttack01");
    private int aniHash_Attack02 = Animator.StringToHash("doAttack02");
    private int aniHash_Walk = Animator.StringToHash("isWalk");


    private FixedJoystick joystick;

    private GameObject obj;



    private void Awake()
    {
        charState = GetComponent<CharState>();
        col = GetComponent<CapsuleCollider>();
        if(!TryGetComponent<Animator>(out anim))
        {
            Debug.Log("MyCharController.cs - Awake() - anim ���� ����");
        }

        obj = GameObject.Find("Fixed Joystick");
        if (obj != null)
        {
            if(!obj.TryGetComponent<FixedJoystick>(out joystick))
            {
                Debug.Log("MyCharController.cs - Awake() - joystick ���� ����");
            }
        }
        else
        {
            Debug.Log("MyCharController.cs - Awake() - ���̽�ƽ�� ���忡 ��ġ���� ����");
        }
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
        move.x += joystick.Horizontal;
        move.z += joystick.Vertical;

        move.Normalize();

        input_Attack01 = Input.GetKeyDown(KeyCode.Space);
        input_Attack02 = Input.GetKeyDown(KeyCode.F);
    }

    void Locomotion()
    {
        transform.position += Time.deltaTime * moveSpeed * move;
        // �̵��ϴ� �������� ȸ��
        transform.LookAt(transform.position + move);
    }

    void CharAnims()
    {
        // ���� �ִϸ��̼�
        if (input_Attack01)
        {
            anim.SetTrigger(aniHash_Attack01);
        }
        else if (input_Attack02) // else if �� �� ������ �� ���� ���ÿ� ���� ���ϰ� �ϱ� ���ؼ�
        {
            anim.SetTrigger(aniHash_Attack02);
        }

        // �̵� �ִϸ��̼�
        // anim.SetBool(); // �ؽ��ڵ� hash code, hash table
        // hash �ڵ�� �Է��ϸ� �޸𸮸� �Ƴ� �� ����
        anim.SetBool(aniHash_Walk, move != Vector3.zero);

    }
}
