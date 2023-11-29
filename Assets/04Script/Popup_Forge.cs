using System.Collections;
using System.Collections.Generic;
using System.Linq; // ���� ����(List, dic)
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Popup_Forge : PopupBase, IPopup
{
    [SerializeField]
    private GameObject forgeSlotPrefab;
    [SerializeField]
    private RectTransform contentRect;

    private Image iconImg;
    [SerializeField]
    private TextMeshProUGUI balanceText;
    [SerializeField]
    private TextMeshProUGUI enchantPrice;
    [SerializeField]
    private TextMeshProUGUI enchantLevel;
    [SerializeField]
    private TextMeshProUGUI enchantAfter;
    [SerializeField]
    private Button applyBtn;
    [SerializeField]
    List<ForgeSlot> forgeSlotList = new List<ForgeSlot>();

    private Inventory inventory;


    private void Awake()
    {
        PopupClose();
        applyBtn.onClick.AddListener(OnClick_Apply);
        InitPopup();
    }


    ForgeSlot slot;
    GameObject obj;
    private void InitPopup()
    {
        inventory = GameManager.Inst.Inven;

        for (int i = 0; i < inventory.MAXITEMCOUNT; i++)
        {
            obj = Instantiate(forgeSlotPrefab, contentRect);
            obj.name = "ForgeSlot_" + i;
            slot = obj.GetComponent<ForgeSlot>();
            slot.CreateSlot(this);
            forgeSlotList.Add(slot);
        }
    }


    // ����Ʈ�� �����ϴ� ����
    List<InventoryitemData> dataList;
    ItemData_Entity data_Entity;

    private void RefreshData()
    {
        balanceText.text = GameManager.Inst.PlayerGold.ToString();

        inventory = GameManager.Inst.Inven;

        // ���� ���縦 ���ؼ� �����͸� ����.
        // dataList = inventory.GetItemList();

        // ���� ���縦 ���ؼ� �����͸� ����
        dataList = inventory.GetItemList().ToList<InventoryitemData>();

        // ���üҿ� ǥ�õǾ�� �ϴ� ������(��ȭ�� ���� �ؾ���)
        for(int i = inventory.CURITEMCOUNT -1; i>=0; i--)
        {
            if(GameManager.Inst.GetItemData(dataList[i].itemID, out data_Entity))
            {
                if(!data_Entity.equip)
                {
                    dataList.RemoveAt(i);
                }
            }
        }

        for(int i =0; i < forgeSlotList.Count; i++)
        {
            if (i < dataList.Count)
            {
                forgeSlotList[i].RefreshSlot(dataList[i]);
            
            }
            else
            {
                // ȭ��ǥ�� X ��Ȱ��
                forgeSlotList[i].ClearSlot();
            }
        }
    }


    // �����۵� �߿��� �ϳ��� �����ϸ�, ������ ������ ������ left â�� ǥ��
    InventoryitemData selectItem;
    public void SelectItem(InventoryitemData itemData)
    {
        for(int i = 0; i < dataList.Count; i++)
        {
            if(dataList[i].uid == itemData.uid)
            {
                selectItem = itemData;
                enchantPrice.text = (itemData.itemID / 1000 * 500).ToString();

                enchantLevel.text = (itemData.itemID % 1000).ToString();
                enchantAfter.text = (itemData.itemID % 1000 + 1).ToString();
            }
            else
            {
                // ���� ����
                forgeSlotList[i].SelectFocus(false);
            }
        }
    }

    // ������ ��ȭ
    // 1. ��ȭ�� �ʿ��� ��ȭ�� ����Ѱ�
    // 2. ��ȭ Ȯ���� ���� �����ߴ°�
    // 3-1. �����ϸ� ��ȭ�� �����ϰ�, �������� ��ȭ.
    // 3-2. �����ϸ� ��ȭ�� ����

    public void OnClick_Apply()
    {
        if (TryEnchant())
        {
            // ����
            selectItem.itemID++;
            GameManager.Inst.Inven.UpdateItemInfo(selectItem); // ������ ����
            SelectItem(selectItem);
        }
        // ����
        else
        {

        }
    }

    // ��ȭ ��ᰡ ������� üũ�ϴ� �Լ�
    private bool CanEnchant()
    {
        // MAX ��ȭ�� �Ϸ�� ������
        if((selectItem.itemID % 1000) >= 5)
        {
            return false;
        }
        // ��尡 ������ ��Ȳ
        if ((selectItem.itemID % 1000) * 500 > GameManager.Inst.PlayerGold)
        {
            return false;
        }

        return true;
    }


    // ��ȭ ���� ���θ� ����
    private bool TryEnchant()
    {
        bool isSuccess = false;

        if (CanEnchant())
        {
            isSuccess = (Random.RandomRange(0, 10000) < 5000);
            GameManager.Inst.PlayerGold -= ((selectItem.itemID % 1000) * 500);
            RefreshData();
        }

        return isSuccess;
    }

    public void PopupClose()
    {
        gameObject.LeanScale(Vector3.zero, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }

    public void PopupOpen()
    {
        // ������ �ֽ� ������ ����
        RefreshData();
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }
}
