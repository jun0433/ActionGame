using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryitemData
{
    public int uid;         // 아이템 고유 ID
    public int itemID;      // 테이블 ID
    public int amount;      // 총 갯수
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
        int index = FindItemIndex(newItem); // 인벤에 있는지, 있으면 몇 번째 index인지 확인

        // 테이블 데이터가 존재하는지 체크
        if (true)
        {
            // 장착이 가능한 아이템인 경우
            if (true)
            {
                // 중복되지 않도록 값 생성
                newItem.uid = 1;
                newItem.amount = 1;
                items.Add(newItem);
                curItemCount++;
            }
            // 중첩이 가능한 아이템 && 이미 가지고 있는 경우
            else if(-1 < index)
            {
                items[index].amount += newItem.amount;
            }
            // 중첩 가능 && 처음 습득한 경우
            else
            {
                newItem.uid = -1;
                items.Add(newItem);
                curItemCount++;
            }
        }
    }

    // 아이템 강화 했을 때, 변경된 정보를 갱신
    public void UpdateItemInfo(InventoryitemData newData)
    {
        for(int i = 0; i< items.Count; i++)
        {
            if (items[i].uid == newData.uid)
            {
                Debug.Log(items[i].itemID + " 강화 처리" + items[i].uid);
                items[i].itemID = newData.itemID;
                items[i].amount = newData.amount;
            }
        }
    }

    public bool IsFull()
    {
        return curItemCount >= maxItemCount;
    }


    // 인벤토리의 중복된 아이템이 있다면, 몇 번째에 있는지 index를 리턴
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


    // 아이템 리스트를 외부에서 확인 할 수 있게 하는 함수
    public List<InventoryitemData> GetItemList()
    {
        CURITEMCOUNT = items.Count; // 보유한 아이템의 갯수를 갱신
        return items;
    }
    

    // 상점에 팔거나, 버리기를 할 때 호출.
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
