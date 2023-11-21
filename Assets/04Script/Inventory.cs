using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryitemData
{
    public int uid;         // ������ ���� ID
    public int itemID;      // ���̺� ID
    public int amount;      // �� ����
}

[System.Serializable]
public class Inventory
{
    private int maxItemCount = 18;
    public int MAXITEMCOUNT
    {
        get => maxItemCount;
    }

    private int curItemCount;
    public int CURITEMCOUNT
    {
        get => curItemCount;
        set => curItemCount = value;
    }

    [SerializeField]
    private List<InventoryitemData> items = new List<InventoryitemData>();

    public void AddItem(InventoryitemData newItem)
    {
        int index = FindItemIndex(newItem); // �κ��� �ִ���, ������ �� ��° index���� Ȯ��

        // ���̺� �����Ͱ� �����ϴ��� üũ
        if (true)
        {
            // ������ ������ �������� ���
            if (true)
            {
                // �ߺ����� �ʵ��� �� ����
                newItem.uid = 1;
                newItem.amount = 1;
                items.Add(newItem);
                curItemCount++;
            }
            // ��ø�� ������ ������ && �̹� ������ �ִ� ���
            else if(-1 < index)
            {
                items[index].amount += newItem.amount;
            }
            // ��ø ���� && ó�� ������ ���
            else
            {
                newItem.uid = -1;
                items.Add(newItem);
                curItemCount++;
            }
        }
    }

    // ������ ��ȭ ���� ��, ����� ������ ����
    public void UpdateItemInfo(InventoryitemData newData)
    {
        for(int i = 0; i< items.Count; i++)
        {
            if (items[i].uid == newData.uid)
            {
                Debug.Log(items[i].itemID + " ��ȭ ó��" + items[i].uid);
                items[i].itemID = newData.itemID;
                items[i].amount = newData.amount;
            }
        }
    }

    public bool IsFull()
    {
        return curItemCount >= maxItemCount;
    }


    // �κ��丮�� �ߺ��� �������� �ִٸ�, �� ��°�� �ִ��� index�� ����
    private int FindItemIndex(InventoryitemData newItem)
    {
        int result = -1;
        for(int i = items.Count -1; i >= 0; i--)
        {
            if(items[i].itemID == newItem.itemID)
            {
                result = i;
                break;
            }
        }
        return result;
    }


    // ������ ����Ʈ�� �ܺο��� Ȯ�� �� �� �ְ� �ϴ� �Լ�
    public List<InventoryitemData> GetItemList()
    {
        CURITEMCOUNT = items.Count; // ������ �������� ������ ����
        return items;
    }
    

    // ������ �Ȱų�, �����⸦ �� �� ȣ��.
    public void DeleteItem(InventoryitemData deleteItem)
    {
        int index = FindItemIndex(deleteItem);

        if(index > -1)
        {
            items[index].amount -= deleteItem.amount;
            if(items[index].amount < 1)
            {
                items.RemoveAt(index);
                curItemCount--;
            }
        }
    }
}
