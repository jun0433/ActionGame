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

    private void onClick_SelectButton()
    {

    }



}
