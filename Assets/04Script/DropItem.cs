using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DropItemType
{
    buffItem,
    inventoryItem,

}

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class DropItem : MonoBehaviour
{
    private Rigidbody rig;
    private SphereCollider col;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();

        col.radius = 0.5f;
        col.isTrigger = true;

        rig.useGravity = true;
        // up�������� 5f��ŭ ���� ��
        rig.AddForce(Vector3.up * 5f, ForceMode.Impulse);

        moveTrans = transform.GetChild(0);
        InitDropitem();

    }



    private bool isDrop;

    private void InitDropitem()
    {
        isDrop = false;
    }

    private Vector3 pos;
    private Transform moveTrans;
    private float dropPosY;
    
    private float valueA;

    private void Update()
    {
        if (isDrop)
        {
            moveTrans.Rotate(Vector3.up * 90.0f * Time.deltaTime);
            pos = moveTrans.position;
            valueA += Time.deltaTime;
            pos.y = dropPosY + 0.3f + Mathf.Sin(valueA);
            moveTrans.position = pos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground")){
            rig.useGravity = false;
            rig.velocity = Vector3.zero;
            dropPosY = moveTrans.position.y;
            isDrop = true;
        }


        if(isDrop && other.CompareTag("Player"))
        {
            // �κ��丮�� ���� ó��
            Destroy(gameObject);
        }
    }
}
