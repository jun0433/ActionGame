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
        // ��ũ��Ʈ�� ������Ʈ�� ���� ���ӿ�����Ʈ�� ���� �� ���
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
        // �ڽ��� �������� �ƴҶ���, �������� ó��
        if (!isSelect)
        {
            // �ڽ��� ���õǾ��ٰ� �θ� �˾����� �˸�
            forgePopup.SelectItem(data);
            SelectFocus(true);
        }
    }

    // ������ ���õ� �� �Լ�
    public void SelectFocus(bool select)
    {
        isSelect = select;
        focus.enabled = select;
    }


    // ������ ���� ����
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

    // ������ �������� ���� �� ȣ���ϴ� �Լ�
    public void ClearSlot()
    {
        gameObject.SetActive(false);
    }
}
