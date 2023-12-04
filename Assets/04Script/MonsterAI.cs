using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AI_State
{
    Idle,       // 제자리 대기
    Roaming,    // 주변 배회
    ReturnHome, // 생성 위치로 복귀
    Chase,      // 공격 대상을 추적
    Attack,     // 공격 대상을 공격
}



public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private MonsterBase monsterBase;

    private GameObject target;

    private Vector3 homePos;
    private Vector3 movePos;

    private AI_State currentState; // 현재 수행하고 있는 AI 상태

    private bool isInit; // isInit true AI 작동 상태, false 멈춘 상태


    // Awake가 어떤 것이 먼저 호출 될 지 모름(상호 참조 유의)
    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        monsterBase = GetComponent<MonsterBase>();

        isInit = false;
    }



    // ai를 작동시키는 함수
    public void StartAI()
    {
        isInit = true;
        currentState = AI_State.Roaming;
        ChangeAIState(AI_State.Roaming);
        target = null;
        homePos = transform.position;

    }

    // AI의 상태값을 전환 시키는 함수
    private void ChangeAIState(AI_State newState)
    {
        if (isInit)
        {
            StopCoroutine(currentState.ToString()); // 현재 상태를 중지
            currentState = newState; // 상태값 변화
            StartCoroutine(currentState.ToString()); // 신규 상태 시작
        }
    }

    private void SetMoveTarget(Vector3 targetPos)
    {

        Debug.Log("이동 목표지점 변경: " + targetPos);
        navAgent.SetDestination(targetPos);
    }

    // 공격하는 타겟을 변경하는 함수
    public void SetTarget(GameObject newTarget)
    {
        if (currentState == AI_State.Idle || currentState == AI_State.Roaming)
        {
            target = newTarget;
            ChangeAIState(AI_State.Chase);

        }
    }

    // 타겟과의 거리를 계산하는 함수
    private float GetDistanceToTarget()
    {
        if(target != null)
        {
            return Vector3.Distance(transform.position, target.transform.position); 
        }

        return -1f;
    }

    // 제자리 대기
    private IEnumerator Idle()
    {
        yield return null;
    }

    // homePos를 기준으로 기준점 주변을 배회하는 상태
    private IEnumerator Roaming()
    {
        yield return null;

        while (true)
        {
            movePos.x = Random.Range(-5f, 5f);
            movePos.y = 0f;
            movePos.z = Random.Range(-5f, 5f);
            SetMoveTarget(homePos + movePos);
            yield return new WaitForSeconds(Random.Range(4, 6));
        }
    }


    // 집으로 돌아가는 함수
    private IEnumerator ReturnHome()
    {
        yield return null;

        SetMoveTarget(homePos);
        while (true)
        {
            yield return new WaitForSeconds(1f);
            // 목표 지점까지의 거리 체크
            if(navAgent.remainingDistance < 3f)
            {
                ChangeAIState(AI_State.Roaming);
            }
        }
    }

    private IEnumerator Chase()
    {
        yield return null;
        while(target != null)
        {
            // mosterbase 구현 후 공격 사거리로 체크
            if(GetDistanceToTarget() < monsterBase.STATE.attackRange)
            {
                ChangeAIState(AI_State.Attack);
            }
            else
            {
                SetMoveTarget(target.transform.position); // 지속적으로 목표 좌표를 변경
            }
            yield return new WaitForSeconds(0.5f);
        }
        // 타겟이 사망하거나 없어져서, 집으로 되돌아 오는 로직
        ChangeAIState(AI_State.ReturnHome);
    }

    private IEnumerator Attack()
    {
        yield return null;

        while (target!=null)
        {
            // 공격 사거리보다 큰 경우
            if(GetDistanceToTarget() > monsterBase.STATE.attackRange)
            {
                ChangeAIState(AI_State.Chase);
            }
            transform.LookAt(target.transform); // 상대방을 향해서 회전
            // 공격 애니메이션 + 데미지 처리
            //monsterBase.AttackTarget(target.GetComponent<ICharBase>()); // 다형성을 구현 대표적인 예시


            yield return new WaitForSeconds(monsterBase.STATE.attackRate); // 공격 주기만큼 대기
        }

        ChangeAIState(AI_State.ReturnHome);
    }


}
