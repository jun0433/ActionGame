using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ItemShopSlot : MonoBehaviour
{

    // slot을 만들어낸 주체
    private Popup_Item shopPopup;

    private Image icon;
    private TextMeshProUGUI itemName;
    private TextMeshProUGUI itemPrice;
    private TextMeshProUGUI tradeCountText;

    private Button leftBtn;
    private Button rightBtn;
    private Button maxBtn;

    // 해당 슬롯에 표현해야 하는 대상 아이템 정보
    private InventoryitemData data;

    private int slotIndex;
    public int SLOTINDEX
    {
        get => slotIndex;
    }

    // 단가 * 거래갯수
    private int totalGold;

    public int TOTALGOLD
    {
        get => totalGold;
    }

    private int priceGold;
    private int tradeMaxCount; // 거래할 수 있는 MAX치 (보유한 갯수, 999)
    private int curCount; // 거래하려고 설정한 갯수.

    private int itemID;

    private GameObject obj;

    private void Awake()
    {
        obj = transform.Find("Icon").gameObject;
        if (obj != null)
        {
            if(!obj.TryGetComponent<Image>(out icon))
            {
                Debug.Log("ItemShopSlot.cs - Awake() - Icon 참조 실패");
            }

        }

        obj = transform.Find("ItemName").gameObject;
        if (obj != null)
        {
            if (!obj.TryGetComponent<TextMeshProUGUI>(out itemName))
            {
                Debug.Log("ItemShopSlot.cs - Awake() - itemName 참조 실패");
            }
        }

        obj = transform.Find("Price").gameObject;
        if (obj != null)
        {
            if (!obj.TryGetComponent<TextMeshProUGUI>(out itemPrice))
            {
                Debug.Log("ItemShopSlot.cs - Awake() - priceGold 참조 실패");
            }
        }

        obj = transform.Find("SellCount").gameObject;
        if (obj != null)
        {
            
            if(!obj.TryGetComponent<TextMeshProUGUI>(out tradeCountText))
            {
                Debug.Log("ItemShopSlot.cs - Awake() - priceGold 참조 실패");
            }
            
        }

        obj = transform.Find("LeftBtn").gameObject;
        if (obj != null)
        {
            if(!obj.TryGetComponent<Button>(out leftBtn))
            {
                Debug.Log("ItemShopSlot.cs - Awake() - leftBtn 참조 실패");
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
                Debug.Log("ItemShopSlot.cs - Awake() - rightBtn 참조 실패");
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
                Debug.Log("ItemShopSlot.cs - Awake() - maxBtn 참조 실패");
            }
            else
            {
                maxBtn.onClick.AddListener(OnClick_MaxBtn);
            }
        }
    }

    public void CreateSlot(Popup_Item shop, int index)
    {
        shopPopup = shop; // 슬롯을 만들어준 객체를 참조
        slotIndex = index; // 몇 번째 슬롯인지
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
            Debug.Log("ItemShopSlot.cs - RefreshSlot() - itemData 참조 실패");
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

        // 해당 슬롯의 최종 거래금액을 생성해낸 팝업 스크립트 객체에 전달

    }

    public void OnClick_RightBtn()
    {
        if(curCount < tradeMaxCount)
        {
            curCount++;
        }
        tradeCountText.text = curCount.ToString();
        totalGold = curCount * priceGold;

        // 해당 슬롯의 최종 거래금액을 생성해낸 팝업 스크립트 객체에 전달

    }
    public void OnClick_MaxBtn()
    {
        curCount = tradeMaxCount;
        tradeCountText.text = curCount.ToString();
        totalGold = curCount * priceGold;

        // 해당 슬롯의 최종 거래금액을 생성해낸 팝업 스크립트 객체에 전달
    }


    // 거래를 수행ㅎ라 때, 거래 dlxp ID, 거래 횟수, 총 거래 금액을 반환
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
