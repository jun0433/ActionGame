using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ItemShopSlot : MonoBehaviour
{

    // slot�� ���� ��ü
    private Popup_Item shopPopup;

    private Image icon;
    private TextMeshProUGUI itemName;
    private TextMeshProUGUI itemPrice;
    private TextMeshProUGUI tradeCountText;

    private Button leftBtn;
    private Button rightBtn;
    private Button maxBtn;

    // �ش� ���Կ� ǥ���ؾ� �ϴ� ��� ������ ����
    private InventoryitemData data;

    private int slotIndex;
    public int SLOTINDEX
    {
        get => slotIndex;
    }

    // �ܰ� * �ŷ�����
    private int totalGold;

    public int TOTALGOLD
    {
        get => totalGold;
    }

    private int priceGold;
    private int tradeMaxCount; // �ŷ��� �� �ִ� MAXġ (������ ����, 999)
    private int curCount; // �ŷ��Ϸ��� ������ ����.

    private int itemID;

    private GameObject obj;

    private void Awake()
    {
        obj = transform.Find("Icon").gameObject;
        if (obj != null)
        {
            if(!obj.TryGetComponent<Image>(out icon))
            {
                Debug.Log("ItemShopSlot.cs - Awake() - Icon ���� ����");
            }

        }

        obj = transform.Find("ItemName").gameObject;
        if (obj != null)
        {
            if (!obj.TryGetComponent<TextMeshProUGUI>(out itemName))
            {
                Debug.Log("ItemShopSlot.cs - Awake() - itemName ���� ����");
            }
        }

        obj = transform.Find("Price").gameObject;
        if (obj != null)
        {
            if (!obj.TryGetComponent<TextMeshProUGUI>(out itemPrice))
            {
                Debug.Log("ItemShopSlot.cs - Awake() - priceGold ���� ����");
            }
        }

        obj = transform.Find("SellCount").gameObject;
        if (obj != null)
        {
            
            if(!obj.TryGetComponent<TextMeshProUGUI>(out tradeCountText))
            {
                Debug.Log("ItemShopSlot.cs - Awake() - priceGold ���� ����");
            }
            
        }

        obj = transform.Find("LeftBtn").gameObject;
        if (obj != null)
        {
            if(!obj.TryGetComponent<Button>(out leftBtn))
            {
                Debug.Log("ItemShopSlot.cs - Awake() - leftBtn ���� ����");
            }
            else
            {
                leftBtn.onClick.AddListener(OnClick_LeftBtn);
            }
        }
        

        obj = transform.Find("RightBtn").gameObject;
        if (obj != null)
        {
            if(!obj.TryGetComponent<Button>(out rightBtn))
            {
                Debug.Log("ItemShopSlot.cs - Awake() - rightBtn ���� ����");
            }
            else
            {
                rightBtn.onClick.AddListener(OnClick_RightBtn);
            }

        }
        

        obj = transform.Find("MaxBtn").gameObject;
        if (obj != null)
        {
            if(!obj.TryGetComponent<Button>(out maxBtn))
            {
                Debug.Log("ItemShopSlot.cs - Awake() - maxBtn ���� ����");
            }
            else
            {
                maxBtn.onClick.AddListener(OnClick_MaxBtn);
            }
        }
    }

    public void CreateSlot(Popup_Item shop, int index)
    {
        shopPopup = shop; // ������ ������� ��ü�� ����
        slotIndex = index; // �� ��° ��������
        gameObject.SetActive(false);
    }


    public void RefreshSlot(InventoryitemData itemInfo)
    {
        gameObject.SetActive(true);
        itemID = itemInfo.itemID;
        tradeMaxCount = itemInfo.amount;
        curCount = 0;
        tradeCountText.text = curCount.ToString();

        if(!GameManager.Inst.GetItemData(itemID, out ItemData_Entity itemData))
        {
            Debug.Log("ItemShopSlot.cs - RefreshSlot() - itemData ���� ����");
            return;
        }

        icon.sprite = Resources.Load<Sprite>(itemData.iconImg);
        icon.enabled = true;

        itemPrice.text = itemData.sellGold.ToString();
        priceGold = itemData.sellGold;
    }

    public void ClearSlot()
    {
        gameObject.SetActive(false);
    }


    public void OnClick_LeftBtn()
    {
        if(curCount > 0)
        {
            curCount--;
        }
        tradeCountText.text = curCount.ToString();
        totalGold = curCount * priceGold;

        // �ش� ������ ���� �ŷ��ݾ��� �����س� �˾� ��ũ��Ʈ ��ü�� ����

    }

    public void OnClick_RightBtn()
    {
        if(curCount < tradeMaxCount)
        {
            curCount++;
        }
        tradeCountText.text = curCount.ToString();
        totalGold = curCount * priceGold;

        // �ش� ������ ���� �ŷ��ݾ��� �����س� �˾� ��ũ��Ʈ ��ü�� ����

    }
    public void OnClick_MaxBtn()
    {
        curCount = tradeMaxCount;
        tradeCountText.text = curCount.ToString();
        totalGold = curCount * priceGold;

        // �ش� ������ ���� �ŷ��ݾ��� �����س� �˾� ��ũ��Ʈ ��ü�� ����
    }


    // �ŷ��� ���ा�� ��, �ŷ� dlxp ID, �ŷ� Ƚ��, �� �ŷ� �ݾ��� ��ȯ
    public bool GetSellCount(out int _sellItemID, out int _sellCount, out int _SellGold)
    {
        _sellItemID = itemID;
        _sellCount = curCount;
        _SellGold = totalGold;
        return true;
    }


    public bool GetBuyCount(out int _buyItemID, out int _buyCount, out int _buyGold)
    {
        _buyItemID = itemID;
        _buyCount = curCount;
        _buyGold = totalGold;
        return true;
    }


}
