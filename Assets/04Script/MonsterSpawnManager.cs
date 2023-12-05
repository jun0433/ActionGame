using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public enum SpawnType
{
    once, // ���������� ���ÿ� ����
    repeat, // ���������� ����

}

public class MonsterSpawnManager : MonoBehaviour
{
    [SerializeField]
    private int maxCount; // �ش� ������ ���ÿ� ��� ���� �� �ִ� ������ �ִ� ��
    private int curCount; // �ش� ������ ���� ����ִ� ����.
    private PoolManager poolManager;
    [SerializeField]
    private SpawnType spawnType; // �ش� ���� ���� ���
    [SerializeField]
    private int spawnMonsterTableID; // �ش� ���� �����ϴ� ���� ������ ���̺� ID;


    private void Awake()
    {
        if(!TryGetComponent<PoolManager>(out poolManager))
        {
            Debug.Log("monsterSpawnManager.cs - Awkae() - poolMnager ���� ����");
        }

        InitSpawnManger();
    }

    // �����ϱ� ���� �ʱ�ȭ
    private void InitSpawnManger()
    {
        curCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���� ����
        if (other.CompareTag("Player"))
        {
            StartCoroutine("TrySpawn");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ���� ����
        if (other.CompareTag("Player"))
        {
            StopCoroutine("TrySpawn");
        }
    }

    IEnumerator TrySpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            if(curCount < maxCount)
            {
                SpawnUnit();
            }
        }
    }

    private Vector3 spawnPos;
    private MonsterBase monsterBase;

    private void SpawnUnit()
    {
        curCount++;

        // GetFromPool<MonsterBase>(index)
        monsterBase = poolManager.GetFromPool<MonsterBase>(0);

        spawnPos.x = transform.position.x + Random.Range(-5f, 5f);
        spawnPos.y = 0f;
        spawnPos.z = transform.position.z + Random.Range(-5f, 5f);

        // ������ �������� spawnPos�� ����
        monsterBase.transform.position = spawnPos;
        monsterBase.Initmonster(spawnMonsterTableID);
    }



    public void ReturnPool(MonsterBase returnMonster)
    {
        poolManager.TakeToPool<MonsterBase>(monsterBase.PoolName, returnMonster);
        curCount--;
    }
}
