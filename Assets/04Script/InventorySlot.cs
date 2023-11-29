using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// resources icon�� �������� �ε��ؼ� ����ϰ� ����
// ��������, ������Ʈ Ǯ�� ����, ���� ���۶� ���� �ε��ߴٰ�
// ��Ȱ���ϴ� ������ �ڵ� �ۼ�

public class InventorySlot : MonoBehaviour
{
    private bool isEmpty; // �󽽷����� üũ�ϴ� ����
    public bool EMTPY
    {
        get => isEmpty;
    }

    private int slotIndex; // �ش� ������ ��ġ�� ��Ÿ���� ����
    public int SLOTINDEX
    {
        get => slotIndex;
        set => slotIndex = value;
    }

    private Image icon; // ������
    private GameObject focus; // foucus ���� ǥ�����ִ� �̹���
    private TextMeshProUGUI amount; // ������ ���� ���� ǥ���ϴ� �ؽ�Ʈ
    private Button button;

    private bool isSelect;


    private void Awake()
    {
        icon = transform.GetChild(0).GetComponent<Image>();
        focus = transform.GetChild(1).gameObject;
        amount = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        button = GetComponent<Button>();

        button.onClick.AddListener(onClick_SelectButton);

    }

    // ������ ������ ��, ������, ���� ���� �����ϴ� �Լ�
    public void DrawItem(InventoryitemData newItem)
    {
        // ���̺� �����Ͱ� ������
        if(GameManager.Inst.GetItemData(newItem.itemID, out ItemData_Entity itemData))
        {
            // Resources �������� ���ҽ� �ε�
            icon.sprite = Resources.Load<Sprite>(itemData.iconImg);
            ChangeAmount(newItem.amount);
            isEmpty = false;
            icon.enabled = true;
        }
        else
        {
            Debug.Log("InventorySlost.cs - DrawItem() - ���̺� �ش��ϴ� ID�� ����: " + newItem.itemID);
        }
    }

    // ������ �������� ��ĭ���� ó���ϴ� �Լ�
    public void ClearSlot()
    {
        focus.SetActive(false);
        isSelect = false;
        amount.enabled = false;
        isEmpty = true;
        icon.enabled= false;
    }
    
    // ���� ������ �ٲ��ִ� �Լ�
    public void ChangeAmount(int newAmount)
    {
        amount.text = newAmount.ToString();
    }

    public void SelectSlot(bool isSelect)
    {
        focus.SetActive(isSelect);
    }

    private void onClick_SelectButton()
    {
        if (!isEmpty)
        {
            isSelect = true;
            SelectSlot(isSelect);
        }
    }



}
