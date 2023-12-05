using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class Projectile : MonoBehaviour, IPoolObject
{

    [SerializeField]
    private float lifeTime; // ���� ���Ŀ� ���忡 ������� �� �ִ� �ð�
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
        lifeTime = Time.time + newLifeTime; // �Ҹ� �� ���� �ð�
        attackDamage = newDamage;
        ownerTag = newTag;
        poolManager = pools;

        isInit = true;
    }


    private void Update()
    {
        // �ʱ�ȭ�� �� ������Ÿ�ϸ�, ���ư��� ó�� ����
        if(isInit)
        {
            transform.position += Time.deltaTime * moveSpeed * moveDir;
            // ���ư��鼭 ȸ��
            transform.Rotate(Time.deltaTime * 720f * -Vector3.forward);
            if(Time.time > lifeTime)
            {
                // �Ҹ�
                Explosion();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ownerTag))
        {
            // �Ҹ�
            Explosion();
        }
    }

    private void Explosion()
    {
        Debug.Log("������Ÿ�� �Ҹ�");
        poolManager.TakeToPool<Projectile>(poolName, this);
    }

    public void OnCreatedInPool()
    {
        
    }

    public void OnGettingFromPool()
    {

    }
}
