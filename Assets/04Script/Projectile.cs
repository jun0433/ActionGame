using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class Projectile : MonoBehaviour, IPoolObject
{

    [SerializeField]
    private float lifeTime; // 스폰 이후에 월드에 살아있을 수 있는 시간
    [SerializeField]
    private string poolName;
    private PoolManager poolManager;
    private string ownerTag;
    private bool isInit;

    private Vector3 moveDir;
    private float moveSpeed;

    private int attackDamage;

    public void InitProjectile(Vector3 newDir, float newSpeed, float newLifeTime, int newDamage, string newTag, PoolManager pools)
    {
        moveDir = newDir;
        moveSpeed = newSpeed;
        lifeTime = Time.time + newLifeTime; // 소멸 될 예정 시간
        attackDamage = newDamage;
        ownerTag = newTag;
        poolManager = pools;

        isInit = true;
    }


    private void Update()
    {
        // 초기화가 된 프로젝타일만, 날아가는 처리 수행
        if(isInit)
        {
            transform.position += Time.deltaTime * moveSpeed * moveDir;
            // 날아가면서 회전
            transform.Rotate(Time.deltaTime * 720f * -Vector3.forward);
            if(Time.time > lifeTime)
            {
                // 소멸
                Explosion();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ownerTag))
        {
            // 소멸
            Explosion();
        }
    }

    private void Explosion()
    {
        Debug.Log("프로젝타일 소멸");
        poolManager.TakeToPool<Projectile>(poolName, this);
    }

    public void OnCreatedInPool()
    {
        
    }

    public void OnGettingFromPool()
    {

    }
}
