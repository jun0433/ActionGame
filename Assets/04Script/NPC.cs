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
            Debug.Log("NPC.cs - Awake() - col 참조 실패");
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
                Debug.Log("NPC.cs - Awake() - targetPopup 참조 실패");
            }
        }
        else
        {
            Debug.Log("NPC.cs - Awake() - 팝업 오브젝트를 찾기 못함");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // isOn이 false 상태고 Player와 접촉이 된다면
        if(!isOn && other.CompareTag("Player"))
        {
            isOn = true;
            targetPopup.PopupOpen();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        // isOn이 false 상태고 Player와 접촉이 끝난다면
        if (isOn && other.CompareTag("Player"))
        {
            isOn = false;
            targetPopup.PopupClose();
        }
    }
}
