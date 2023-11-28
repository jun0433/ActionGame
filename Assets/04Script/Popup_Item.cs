using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ���� ��� �Ұ�(�������̽��� ���)
public class Popup_Item : PopupBase, IPopup
{

    [SerializeField]
    private GameObject slotPrefab;
    [SerializeField]
    private RectTransform sellRect;
    [SerializeField]
    private RectTransform buyRect;
    private TextMeshProUGUI balanceText; // ������ ������ �ִ� ����
    private TextMeshProUGUI amountText; // ������ �ŷ��Ϸ��� ����� �Ѿ�
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

        // ���� �ǿ� ����ؾ��ϴ� ���Ե��� ����        inventory = GameManager.Inst.Inven;
        for(int i =0; i < inventory.MAXITEMCOUNT; i++)
        {
            obj = Instantiate(slotPrefab, sellRect);
            Debug.Log(obj.gameObject.name + " ������Ʈ ���� Ȯ��");
            shopSlot = obj.GetComponent<ItemShopSlot>();
            if (shopSlot == null)
                Debug.Log("��ũ��Ʈ Ȯ��");
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
        // ���� ����.
        RefreshData();
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }

    List<InventoryitemData> dataList;
    int tradeTotalGold;

    // �˾� ���� ������ �������ִ� �Լ�
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
    // ���� �ǿ� ���� ���Ե��� �ŷ��Ϸ��� ����ϰ� �ִ� �ݾ��� �Ѿ�
    public void CalculateGold()
    {
        totalGold = 0;

        // �Ǹ����� ���� ��Ȳ
        if (sellView.activeSelf)
        {
            for (int i = 0; i < sellSlotList.Count; i++)
            {
                // �ش� ������ Ȱ��ȭ �� ��Ȳ�̶��
                if (sellSlotList[i].isActiveAndEnabled)
                {
                    totalGold += sellSlotList[i].TOTALGOLD;
                }
            }
        }
        // �������� ���� ��Ȳ
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

    // UI�� ǥ�õǴ� �ŷ� �ݾ��� �ֽ�ȭ �����ִ� �Լ�
    public void RefreshGold()
    {
        CalculateGold();
        amountText.text = totalGold.ToString();
    }


    InventoryitemData data = new InventoryitemData();
    // NPC�� �Ǹ��ϴ� ����� �����ϴ� �Լ�
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
        // ������ �ֽ� ������ ����
        RefreshData();
        sellView.SetActive(true);
        buyView.SetActive(false);
    }
    
    public void OnClick_BuyTap()
    {
        // ���� ����Ʈ �ֽ� ������ ����
        RefreshBuyList();
        sellView.SetActive(false);
        buyView.SetActive(true);
    }


    // ������� ����� ���������� �ŷ���Ű�� �Լ�
    public void OnClick_Apply()
    {
        // �Ǹ� ����� ���� ��Ȳ
        if (sellView.activeSelf)
        {
            for (int i = inventory.CURITEMCOUNT - 1; i >= 0; i--)
            {
                sellSlotList[i].GetSellCount(out int id, out int count, out int gold);

                GameManager.Inst.PlayerGold += gold;
                data.itemID = id;
                data.amount = count;
                inventory.DeleteItem(data); // data�� �ش��ϴ� �������� ����
            }
        }
        // ���� ����� ���� ��Ȳ
        else
        {
            int buyTotalGold = 0;

            for (int i = 0; i < 4; i++)
            {
                buySlotList[i].GetBuyCount(out int id, out int count, out int gold);
                buyTotalGold += gold;
                
            }
            // �ܰ� ������� Ȯ��
            if(buyTotalGold <= GameManager.Inst.PlayerGold)
            {
                GameManager.Inst.PlayerGold -= buyTotalGold; // ��� �Ҹ�.

                // ���� ������ �����ϴ� ����
                for(int i = 0; i < 4; i++)
                {
                    buySlotList[i].GetBuyCount(out int id, out int count, out int gold);
                    if(count > 0)
                    {
                        data.itemID = id;
                        data.amount = count;
                        inventory.AddItem(data); // ������ ����
                    }
                }
            }
            else
            {
                Debug.Log("�ܰ� �������� ���౸�� ����");
            }
        }
        RefreshData(); // ����â �ֽ� ������ ����
    }

}
