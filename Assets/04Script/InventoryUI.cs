using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    // ���� �Ŵ����κ��� ������ �޾ƿ�
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
    List<InventoryitemData> dataList; // ������ �ִ� �������� ����Ʈ�� �޾ƿͼ� �ӽ� �����ϴ� ���

    private void Awake()
    {
        inventory = GameManager.Inst.Inven;
        //���� ����
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

    // ������ �ֽ��� ������ ���ؼ� �ٽ� �׸���.
    public void RefreshIcon()
    {
        // Inventory�� ������ ����
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
