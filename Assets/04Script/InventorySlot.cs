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

    private void onClick_SelectButton()
    {

    }



}
