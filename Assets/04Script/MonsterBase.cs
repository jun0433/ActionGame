using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



// ������ �������ͽ� ����
// ���� obj�� �Ѹ� ��ũ��Ʈ ����
// �ִϸ��̼� ó��
// ������Ʈ Ǯ ó��

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

public class MonsterBase : MonoBehaviour
{
    private MonsterAI monsterAI;
    private NavMeshAgent agent;
    private Animator anim;
    private Material mater;
    protected UniteState state = new UniteState();

    // �б� �������� state�� ����(���ٽ�)
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

        mater.color = Color.black;

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
        // �׺� ������Ʈ �����Ͽ� �̵��ӵ� ����
        agent.speed = monsterData.moveSpeed;
        agent.stoppingDistance = state.attackRange * 0.7f;



        monsterAI.StartAI();

        // ���� ���� �÷��̾ �ִ� ���·� ������ �� ��, �÷��̾� �ν��� �ȵ� �� ����
        // ���� �ڵ�� �������� ���� �ֺ��� ���� ��ĵ
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
        // sqrMagnitue�� �̿��Ͽ� velocity�� ���� ������
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            // �̵� �ִϸ��̼� ���
        }
        else
        {
            // �̵� �ִϸ��̼� ����. idle anim ����
        }
    }

    /*virtual public void AttackTarget(ICharBse newTarget)
    {
        anim.SetTrigger(hash_attack);
        damageTarget = newTarget;
    }*/

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
                anim.SetTrigger(hash_Die);
            }
        }
        else
        {
            anim.SetTrigger(hash_Hit);
        }
    }
}
