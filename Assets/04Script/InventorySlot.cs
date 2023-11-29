using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// resources icon은 동적으로 로딩해서 사용하게 제작
// 포폴에는, 오브젝트 풀과 같이, 게임 시작때 전부 로딩했다가
// 재활용하는 식으로 코드 작성

public class InventorySlot : MonoBehaviour
{
    private bool isEmpty; // 빈슬롯인지 체크하는 변수
    public bool EMTPY
    {
        get => isEmpty;
    }

    private int slotIndex; // 해당 슬롯의 위치를 나타내는 변수
    public int SLOTINDEX
    {
        get => slotIndex;
        set => slotIndex = value;
    }

    private Image icon; // 아이콘
    private GameObject focus; // foucus 여부 표시해주는 이미지
    private TextMeshProUGUI amount; // 아이템 보유 갯수 표시하는 텍스트
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

    // 슬롯이 보여질 때, 아이콘, 갯수 등을 갱신하는 함수
    public void DrawItem(InventoryitemData newItem)
    {
        // 테이블에 데이터가 있으면
        if(GameManager.Inst.GetItemData(newItem.itemID, out ItemData_Entity itemData))
        {
            // Resources 폴더에서 리소스 로딩
            icon.sprite = Resources.Load<Sprite>(itemData.iconImg);
            ChangeAmount(newItem.amount);
            isEmpty = false;
            icon.enabled = true;
        }
        else
        {
            Debug.Log("InventorySlost.cs - DrawItem() - 테이블에 해당하는 ID는 없다: " + newItem.itemID);
        }
    }

    // 슬롯의 아이콘을 빈칸으로 처리하는 함수
    public void ClearSlot()
    {
        focus.SetActive(false);
        isSelect = false;
        amount.enabled = false;
        isEmpty = true;
        icon.enabled= false;
    }
    
    // 보유 개수를 바꿔주는 함수
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
