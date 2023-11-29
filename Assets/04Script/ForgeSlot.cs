using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForgeSlot : MonoBehaviour
{
    private Popup_Forge forgePopup;
    private Image icon;
    private Image focus;
    private bool isSelect;

    private InventoryitemData data;
    private Button selectBtn;

    private GameObject obj;

    private void Awake()
    {
        // 스크립트와 컴포넌트가 같은 게임오브젝트에 있을 때 사용
        selectBtn = GetComponent<Button>();

        selectBtn.onClick.AddListener(OnClick_SelectBtn);

        obj = transform.Find("Icon").gameObject;
        icon = obj.GetComponent<Image>();

        obj = transform.Find("Focus").gameObject;
        focus = obj.GetComponent<Image>();
    }

    public void CreateSlot(Popup_Forge forge)
    {
        forgePopup = forge;
        gameObject.SetActive(false);

    }

    public void OnClick_SelectBtn()
    {
        // 자신이 선택중이 아닐때만, 선택으로 처리
        if (!isSelect)
        {
            // 자신이 선택되었다고 부모 팝업에게 알림
            forgePopup.SelectItem(data);
            SelectFocus(true);
        }
    }

    // 아이템 선택될 때 함수
    public void SelectFocus(bool select)
    {
        isSelect = select;
        focus.enabled = select;
    }


    // 아이템 정보 갱신
    public void RefreshSlot(InventoryitemData itemData)
    {
        gameObject.SetActive(true);
        data = itemData;

        if(GameManager.Inst.GetItemData(data.itemID, out ItemData_Entity tableData))
        {
            icon.sprite = Resources.Load<Sprite>(tableData.iconImg);
            icon.enabled = true;
            isSelect = false;
            focus.enabled = false;
        }
    }

    // 보여줄 아이템이 없을 때 호출하는 함수
    public void ClearSlot()
    {
        gameObject.SetActive(false);
    }
}
