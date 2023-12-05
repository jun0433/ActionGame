using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using UnityEngine.AI;


public interface ICharBase
{
    public void TakeDamage(int damage); // 순수 가상 함수
}


// 몬스터의 스테이터스 관리
// 몬스터 obj의 뿌리 스크립트 역할
// 애니메이션 처리
// 오브젝트 풀 처리


public class UniteState
{
    public int currentHP;
    public int maxHP;
    public int defence;
    public int attackDamage;
    public float attackRange;
    public float attackRate;
    public float moveSpeed;
    public int dropEXP;

    public int CalculateDamage(int takeDamage)
    {
        int result = takeDamage - defence;
        return result > 0 ? result : 1;
    }
}

public class MonsterBase : MonoBehaviour, ICharBase, IPoolObject
{
    private MonsterAI monsterAI;
    private NavMeshAgent agent;
    private Animator anim;
    private Material mater;
    protected UniteState state = new UniteState();

    // 읽기 전용으로 state를 접근(람다식)
    public UniteState STATE => state;


    private int hash_Run = Animator.StringToHash("Run Forward");
    private int hash_Die = Animator.StringToHash("Die");
    private int hash_Hit = Animator.StringToHash("Take Damage");
    private int hash_Attack = Animator.StringToHash("Attack 01");

    private void Awake()
    {
        monsterAI = GetComponent<MonsterAI>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        mater = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;

        Initmonster(101001);
    }

    virtual public void Initmonster(int tableID)
    {
        GameManager.Inst.GetMonsterData(tableID, out MonsterData monsterData);

        state.currentHP = monsterData.maxHP;
        state.maxHP = monsterData.maxHP;
        state.defence = 2;
        state.attackDamage = monsterData.attackDamage;
        state.attackRange = 2f;
        state.attackRate = 2f;
        state.dropEXP = monsterData.EXP;
        // 네비 에이전트 참조하여 이동속도 설정
        agent.speed = monsterData.moveSpeed;
        agent.stoppingDistance = state.attackRange * 0.7f;



        monsterAI.StartAI();

        // 범위 내에 플레이어가 있는 상태로 스폰이 될 때, 플레이어 인식이 안될 수 있음
        // 예외 코드로 스폰하자 마자 주변을 강제 스캔
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Player"));

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player"))
            {
                monsterAI.SetTarget(colliders[i].gameObject);
            }
        }
    }


    private void Update()
    {
        Locomotion();
    }

    private void Locomotion()
    {
        // sqrMagnitue를 이용하여 velocity의 값을 가져옴
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            // 이동 애니메이션 재생
        }
        else
        {
            // 이동 애니메이션 정지. idle anim 시작
        }
    }

    virtual public void AttackTarget(ICharBase newTarget)
    {
        anim.SetTrigger(hash_Attack);
        //damageTarget = newTarget;
    }

    public void ApplyDamgage()
    {
        //damaegeTarget.TakeDamage(state.attackDamage);
    }

    public void TakeDamage(int damage)
    {
        if (state.currentHP > 0)
        {
            state.currentHP -= state.CalculateDamage(damage);
            if (state.currentHP < 0)
            {
                StartCoroutine(OnDie());
            }
        }
        else
        {
            StartCoroutine(OnHit());
        }
    }


    // 피격했을 때 처리하는 효과
    IEnumerator OnHit()
    {
        anim.SetTrigger(hash_Hit);
        for (int i =0; i < 3; i++)
        {
            mater.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            mater.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator OnDie()
    {
        gameObject.layer = LayerMask.NameToLayer("DieChar"); // 충돌 발생 방지
        mater.color = Color.gray;
        anim.SetTrigger(hash_Die);
        yield return new WaitForSeconds(2f);
        // 오브젝트 풀에 반환처리
    }

    [SerializeField]
    private string poolName;
    
    public string PoolName
    {
        get => poolName;
    }

    public void OnCreatedInPool()
    {
        // 최초 객체가 생성되고 오브젝트풀에 담겨질때 1회성으로 호출되는 이벤트.
        Debug.Log("객체 생성" + gameObject.name);
    }

    public void OnGettingFromPool()
    {
        // 오브젝트 풀에서 꺼내질때 호출되는 이벤트
        Debug.Log("객체 불러오기" + gameObject.name);
    }

    // 공격모션
}
