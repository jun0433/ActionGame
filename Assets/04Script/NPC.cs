using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPC_Type
{
    NT_ItemShop,
    NT_Forge,

}

[RequireComponent(typeof(SphereCollider))]

public class NPC : MonoBehaviour
{
    [SerializeField]
    private NPC_Type type;
    private bool isOn = false;
    private SphereCollider col;

    [SerializeField]
    private IPopup targetPopup;
    private GameObject obj;
    [SerializeField]
    private string popupObjName = "None";


    private void Awake()
    {
        if(!TryGetComponent<SphereCollider>(out col)){
            Debug.Log("NPC.cs - Awake() - col ���� ����");
        }
        else
        {
            col.radius = 2.0f;
            col.isTrigger = true;
        }

        obj = GameObject.Find(popupObjName);
        if (obj != null)
        {
            if (!obj.TryGetComponent<IPopup>(out targetPopup))
            {
                Debug.Log("NPC.cs - Awake() - targetPopup ���� ����");
            }
        }
        else
        {
            Debug.Log("NPC.cs - Awake() - �˾� ������Ʈ�� ã�� ����");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // isOn�� false ���°� Player�� ������ �ȴٸ�
        if(!isOn && other.CompareTag("Player"))
        {
            isOn = true;
            targetPopup.PopupOpen();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        // isOn�� false ���°� Player�� ������ �����ٸ�
        if (isOn && other.CompareTag("Player"))
        {
            isOn = false;
            targetPopup.PopupClose();
        }
    }
}
