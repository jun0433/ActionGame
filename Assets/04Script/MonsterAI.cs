using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AI_State
{
    Idle,       // ���ڸ� ���
    Roaming,    // �ֺ� ��ȸ
    ReturnHome, // ���� ��ġ�� ����
    Chase,      // ���� ����� ����
    Attack,     // ���� ����� ����
}



public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private MonsterBase monsterBase;

    private GameObject target;

    private Vector3 homePos;
    private Vector3 movePos;

    private AI_State currentState; // ���� �����ϰ� �ִ� AI ����

    private bool isInit; // isInit true AI �۵� ����, false ���� ����


    // Awake�� � ���� ���� ȣ�� �� �� ��(��ȣ ���� ����)
    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        monsterBase = GetComponent<MonsterBase>();

        isInit = false;
    }



    // ai�� �۵���Ű�� �Լ�
    public void StartAI()
    {
        isInit = true;
        currentState = AI_State.Roaming;
        ChangeAIState(AI_State.Roaming);
        target = null;
        homePos = transform.position;

    }

    // AI�� ���°��� ��ȯ ��Ű�� �Լ�
    private void ChangeAIState(AI_State newState)
    {
        if (isInit)
        {
            StopCoroutine(currentState.ToString()); // ���� ���¸� ����
            currentState = newState; // ���°� ��ȭ
            StartCoroutine(currentState.ToString()); // �ű� ���� ����
        }
    }

    private void SetMoveTarget(Vector3 targetPos)
    {

        Debug.Log("�̵� ��ǥ���� ����: " + targetPos);
        navAgent.SetDestination(targetPos);
    }

    // �����ϴ� Ÿ���� �����ϴ� �Լ�
    public void SetTarget(GameObject newTarget)
    {
        if (currentState == AI_State.Idle || currentState == AI_State.Roaming)
        {
            target = newTarget;
            ChangeAIState(AI_State.Chase);

        }
    }

    // Ÿ�ٰ��� �Ÿ��� ����ϴ� �Լ�
    private float GetDistanceToTarget()
    {
        if(target != null)
        {
            return Vector3.Distance(transform.position, target.transform.position); 
        }

        return -1f;
    }

    // ���ڸ� ���
    private IEnumerator Idle()
    {
        yield return null;
    }

    // homePos�� �������� ������ �ֺ��� ��ȸ�ϴ� ����
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


    // ������ ���ư��� �Լ�
    private IEnumerator ReturnHome()
    {
        yield return null;

        SetMoveTarget(homePos);
        while (true)
        {
            yield return new WaitForSeconds(1f);
            // ��ǥ ���������� �Ÿ� üũ
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
            // mosterbase ���� �� ���� ��Ÿ��� üũ
            if(GetDistanceToTarget() < monsterBase.STATE.attackRange)
            {
                ChangeAIState(AI_State.Attack);
            }
            else
            {
                SetMoveTarget(target.transform.position); // ���������� ��ǥ ��ǥ�� ����
            }
            yield return new WaitForSeconds(0.5f);
        }
        // Ÿ���� ����ϰų� ��������, ������ �ǵ��� ���� ����
        ChangeAIState(AI_State.ReturnHome);
    }

    private IEnumerator Attack()
    {
        yield return null;

        while (target!=null)
        {
            // ���� ��Ÿ����� ū ���
            if(GetDistanceToTarget() > monsterBase.STATE.attackRange)
            {
                ChangeAIState(AI_State.Chase);
            }
            transform.LookAt(target.transform); // ������ ���ؼ� ȸ��
            // ���� �ִϸ��̼� + ������ ó��
            //monsterBase.AttackTarget(target.GetComponent<ICharBase>()); // �������� ���� ��ǥ���� ����


            yield return new WaitForSeconds(monsterBase.STATE.attackRate); // ���� �ֱ⸸ŭ ���
        }

        ChangeAIState(AI_State.ReturnHome);
    }


}
