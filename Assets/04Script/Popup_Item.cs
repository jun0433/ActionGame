using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 다중 상속 불가(인터페이스는 허용)
public class Popup_Item : PopupBase, IPopup
{

    [SerializeField]
    private GameObject slotPrefab;
    [SerializeField]
    private RectTransform sellRect;
    [SerializeField]
    private RectTransform buyRect;
    private TextMeshProUGUI balanceText; // 유저가 가지고 있는 금팩
    private TextMeshProUGUI amountText; // 유저가 거래하려고 등록한 총액
    private Inventory inventory;
    private ItemShopSlot shopSlot;

    List<ItemShopSlot> sellSlotList = new List<ItemShopSlot>();
    List<ItemShopSlot> buySlotList = new List<ItemShopSlot>();

    private GameObject sellView;
    private GameObject buyView;

    private GameObject obj;

    private Button sellTapBtn;
    private Button buyTapBtn;
    private Button applyBtn;

    private void Awake()
    {
        //obj = GameObject.Find("SellTapView").transform.GetChild(0).GetChild(0).gameObject;
        //sellRect = (RectTransform)obj.transform;

        //obj = GameObject.Find("BuyTapView").transform.GetChild(0).GetChild(0).gameObject;
        //buyRect = (RectTransform)obj.transform;

        obj = GameObject.Find("PlayerBalance");
        balanceText = obj.GetComponent<TextMeshProUGUI>();

        obj = GameObject.Find("AmountText");
        amountText = obj.GetComponent<TextMeshProUGUI>();

        sellView = GameObject.Find("SellTabView");
        buyView = GameObject.Find("BuyTabView");

        // 각각 탭에 사용해야하는 슬롯등을 생성        inventory = GameManager.Inst.Inven;
        for(int i =0; i < inventory.MAXITEMCOUNT; i++)
        {
            obj = Instantiate(slotPrefab, sellRect);
            Debug.Log(obj.gameObject.name + " 오브젝트 생성 확인");
            shopSlot = obj.GetComponent<ItemShopSlot>();
            if (shopSlot == null)
                Debug.Log("스크립트 확인");
            shopSlot.CreateSlot(this, i);
            obj.name = "SellSlot_" + i;
            sellSlotList.Add(shopSlot);
        }

        for (int i = 0; i <4; i++)
        {
            obj = Instantiate(slotPrefab, buyRect);
            shopSlot = obj.GetComponent<ItemShopSlot>();
            shopSlot.CreateSlot(this, i);
            obj.name = "BuySlot_" + i;
            buySlotList.Add(shopSlot);
        }


        obj = GameObject.Find("SellTapBtn");
        sellTapBtn = obj.GetComponent<Button>();
        sellTapBtn.onClick.AddListener(OnClick_SellTap);
        obj = GameObject.Find("BuyTapBtn");
        buyTapBtn = obj.GetComponent<Button>();
        buyTapBtn.onClick.AddListener(OnClick_BuyTap);
        obj = GameObject.Find("ApplyBtn");
        applyBtn = obj.GetComponent<Button>();
        applyBtn.onClick.AddListener(OnClick_Apply);

        //gameObject.SetActive(false);
        PopupClose();
    }

    public void PopupClose()
    {
        gameObject.LeanScale(Vector3.zero, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }

    public void PopupOpen()
    {
        // 정보 갱신.
        RefreshData();
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }

    List<InventoryitemData> dataList;
    int tradeTotalGold;

    // 팝업 최초 정보를 갱신해주는 함수
    private void RefreshData()
    {
        balanceText.text = GameManager.Inst.PlayerGold.ToString();
        inventory = GameManager.Inst.Inven;

        dataList = inventory.GetItemList();

        for(int i = 0; i < inventory.MAXITEMCOUNT; i++)
        {
            if(i < inventory.CURITEMCOUNT && -1 < dataList[i].itemID)
            {
                sellSlotList[i].RefreshSlot(dataList[i]);
            }
            else
            {
                sellSlotList[i].ClearSlot();
            }
        }
        tradeTotalGold = 0;
        amountText.text = tradeTotalGold.ToString();
    }

    private int totalGold;
    // 열린 탭에 속한 슬롯들이 거래하려고 대기하고 있는 금액의 총액
    public void CalculateGold()
    {
        totalGold = 0;

        // 판매탭이 열린 상황
        if (sellView.activeSelf)
        {
            for (int i = 0; i < sellSlotList.Count; i++)
            {
                // 해당 슬롯이 활성화 된 상황이라면
                if (sellSlotList[i].isActiveAndEnabled)
                {
                    totalGold += sellSlotList[i].TOTALGOLD;
                }
            }
        }
        // 구매탭이 열린 상황
        else
        {
            for (int i = 0; i < buySlotList.Count; i++)
            {
                if (buySlotList[i].isActiveAndEnabled)
                {
                    totalGold += buySlotList[i].TOTALGOLD;
                }
            }
        }
    }

    // UI에 표시되는 거래 금액을 최신화 시켜주는 함수
    public void RefreshGold()
    {
        CalculateGold();
        amountText.text = totalGold.ToString();
    }


    InventoryitemData data = new InventoryitemData();
    // NPC가 판매하는 목록을 갱신하는 함수
    private void RefreshBuyList()
    {
        for(int i = 0; i <4; i++)
        {
            data.itemID = 2001001 + i;
            data.amount = 999;

            buySlotList[i].RefreshSlot(data);
        }
    }

    public void OnClick_SellTap()
    {
        // 슬롯을 최신 정보로 세팅
        RefreshData();
        sellView.SetActive(true);
        buyView.SetActive(false);
    }
    
    public void OnClick_BuyTap()
    {
        // 물약 리스트 최신 정보로 세팅
        RefreshBuyList();
        sellView.SetActive(false);
        buyView.SetActive(true);
    }


    // 만들어진 목록을 최종적으로 거래시키는 함수
    public void OnClick_Apply()
    {
        // 판매 목록이 열린 상황
        if (sellView.activeSelf)
        {
            for (int i = inventory.CURITEMCOUNT - 1; i >= 0; i--)
            {
                sellSlotList[i].GetSellCount(out int id, out int count, out int gold);

                GameManager.Inst.PlayerGold += gold;
                data.itemID = id;
                data.amount = count;
                inventory.DeleteItem(data); // data에 해당하는 아이템을 제거
            }
        }
        // 구매 목록이 열린 상황
        else
        {
            int buyTotalGold = 0;

            for (int i = 0; i < 4; i++)
            {
                buySlotList[i].GetBuyCount(out int id, out int count, out int gold);
                buyTotalGold += gold;
                
            }
            // 잔고가 충분한지 확인
            if(buyTotalGold <= GameManager.Inst.PlayerGold)
            {
                GameManager.Inst.PlayerGold -= buyTotalGold; // 골드 소모.

                // 실제 물약을 지급하는 로직
                for(int i = 0; i < 4; i++)
                {
                    buySlotList[i].GetBuyCount(out int id, out int count, out int gold);
                    if(count > 0)
                    {
                        data.itemID = id;
                        data.amount = count;
                        inventory.AddItem(data); // 아이템 지급
                    }
                }
            }
            else
            {
                Debug.Log("잔고 부족으로 물약구매 실패");
            }
        }
        RefreshData(); // 상점창 최신 정보로 갱신
    }

}
