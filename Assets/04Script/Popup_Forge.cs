using System.Collections;
using System.Collections.Generic;
using System.Linq; // 깊은 복사(List, dic)
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


    // 리스트를 참조하는 변수
    List<InventoryitemData> dataList;
    ItemData_Entity data_Entity;

    private void RefreshData()
    {
        balanceText.text = GameManager.Inst.PlayerGold.ToString();

        inventory = GameManager.Inst.Inven;

        // 얕은 복사를 통해서 데이터를 복사.
        // dataList = inventory.GetItemList();

        // 깊은 복사를 통해서 데이터를 복사
        dataList = inventory.GetItemList().ToList<InventoryitemData>();

        // 제련소에 표시되어야 하는 아이템(강화가 가능 해야함)
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
                // 화면표시 X 비활성
                forgeSlotList[i].ClearSlot();
            }
        }
    }


    // 장착템들 중에서 하나를 선택하면, 선택한 아이템 정보를 left 창에 표기
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
                // 선택 해제
                forgeSlotList[i].SelectFocus(false);
            }
        }
    }

    // 아이템 강화
    // 1. 강화에 필요한 재화가 충분한가
    // 2. 강화 확률에 의해 성공했는가
    // 3-1. 성공하면 재화를 제거하고, 아이템을 강화.
    // 3-2. 실패하면 재화만 제거

    public void OnClick_Apply()
    {
        if (TryEnchant())
        {
            // 성공
            selectItem.itemID++;
            GameManager.Inst.Inven.UpdateItemInfo(selectItem); // 데이터 갱신
            SelectItem(selectItem);
        }
        // 실패
        else
        {

        }
    }

    // 강화 재료가 충분한지 체크하는 함수
    private bool CanEnchant()
    {
        // MAX 강화가 완료된 아이템
        if((selectItem.itemID % 1000) >= 5)
        {
            return false;
        }
        // 골드가 부족한 상황
        if ((selectItem.itemID % 1000) * 500 > GameManager.Inst.PlayerGold)
        {
            return false;
        }

        return true;
    }


    // 강화 성공 여부를 리턴
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
        // 슬롯을 최신 정보로 갱신
        RefreshData();
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }
}
