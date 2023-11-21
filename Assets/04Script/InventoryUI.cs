using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    // 게임 매니저로부터 데이터 받아옴
    private Inventory inventory;
    [SerializeField]
    private ScrollRect scrollRect;

    List<InventorySlot> slotList = new List<InventorySlot>();

    [SerializeField]
    private GameObject slotPrefabs;
    [SerializeField]
    RectTransform contentTrans;

    InventorySlot slot;
    GameObject obj;
    List<InventoryitemData> dataList; // 가지고 있는 아이템의 리스트를 받아와서 임시 저장하는 장소

    private void Awake()
    {
        inventory = GameManager.Inst.Inven;
        //슬롯 생성
        InitSlots();
    }


    private void InitSlots()
    {
        for(int i = 0; i < inventory.MAXITEMCOUNT; i++)
        {
            slot = Instantiate(slotPrefabs, contentTrans).GetComponent<InventorySlot>();
            slot.SLOTINDEX = i;
            slot.gameObject.name = slot.gameObject.name + i + 1;
            slotList.Add(slot);
        }
    }

    // 슬롯을 최신의 정보를 통해서 다시 그린다.
    public void RefreshIcon()
    {
        // Inventory에 정보를 갱신
        inventory = GameManager.Inst.Inven;
        dataList = inventory.GetItemList();
        inventory.CURITEMCOUNT = dataList.Count;

        for(int i = 0; i < inventory.MAXITEMCOUNT; i++)
        {
            if(i < inventory.CURITEMCOUNT && -1 < dataList[i].itemID)
            {
                slotList[i].DrawItem(dataList[i]);
            }
            else
            {
                slotList[i].ClearSlot();
            }


            slotList[i].SelectSlot(false);
        }
    }

}
