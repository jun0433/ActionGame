using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public enum SpawnType
{
    once, // 여러마리를 동시에 스폰
    repeat, // 지속적으로 스폰

}

public class MonsterSpawnManager : MonoBehaviour
{
    [SerializeField]
    private int maxCount; // 해당 영역에 동시에 살아 있을 수 있는 몬스터의 최대 수
    private int curCount; // 해당 영역에 현재 살아있는 숫자.
    private PoolManager poolManager;
    [SerializeField]
    private SpawnType spawnType; // 해당 영역 스폰 방식
    [SerializeField]
    private int spawnMonsterTableID; // 해당 영역 스폰하는 몬스터 데이터 테이블 ID;


    private void Awake()
    {
        if(!TryGetComponent<PoolManager>(out poolManager))
        {
            Debug.Log("monsterSpawnManager.cs - Awkae() - poolMnager 참조 실패");
        }

        InitSpawnManger();
    }

    // 스폰하기 위한 초기화
    private void InitSpawnManger()
    {
        curCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 스폰 시작
        if (other.CompareTag("Player"))
        {
            StartCoroutine("TrySpawn");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 스폰 종료
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

        // 몬스터의 포지션을 spawnPos로 변경
        monsterBase.transform.position = spawnPos;
        monsterBase.Initmonster(spawnMonsterTableID);
    }



    public void ReturnPool(MonsterBase returnMonster)
    {
        poolManager.TakeToPool<MonsterBase>(monsterBase.PoolName, returnMonster);
        curCount--;
    }
}
