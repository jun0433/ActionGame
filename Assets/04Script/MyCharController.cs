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
            Debug.Log("MyCharController.cs - Awake() - anim 참조 실패");
        }

        obj = GameObject.Find("Fixed Joystick");
        if (obj != null)
        {
            if(!obj.TryGetComponent<FixedJoystick>(out joystick))
            {
                Debug.Log("MyCharController.cs - Awake() - joystick 참조 실패");
            }
        }
        else
        {
            Debug.Log("MyCharController.cs - Awake() - 조이스틱이 월드에 배치되지 않음");
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
        // 이동하는 방향으로 회전
        transform.LookAt(transform.position + move);
    }

    void CharAnims()
    {
        // 공격 애니메이션
        if (input_Attack01)
        {
            anim.SetTrigger(aniHash_Attack01);
        }
        else if (input_Attack02) // else if 를 쓴 이유는 두 개를 동시에 쓰지 못하게 하기 위해서
        {
            anim.SetTrigger(aniHash_Attack02);
        }

        // 이동 애니메이션
        // anim.SetBool(); // 해쉬코드 hash code, hash table
        // hash 코드로 입력하면 메모리를 아낄 수 있음
        anim.SetBool(aniHash_Walk, move != Vector3.zero);

    }
}
