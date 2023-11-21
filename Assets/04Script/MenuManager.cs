using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MenuManager : MonoBehaviour
{
    private GameObject inventoryObj;
    private InventoryUI inventoryUI;
    private bool isOpenInventory;

    private TextMeshProUGUI level;
    private TextMeshProUGUI nickName;
    private Image EXP;
    private Image HP;
    private Image MP;

    private Button menuBtn;
    private Button invenBtn;

    private GameObject obj;


    private void Awake()
    {
        inventoryObj = GameObject.Find("Inventory");
        inventoryUI = inventoryObj.GetComponent<InventoryUI>();
        isOpenInventory = false;
        inventoryObj.LeanScale(Vector3.zero, 0.01f);

        obj = GameObject.Find("InventoryBtn");
        invenBtn = obj.GetComponent<Button>();
        invenBtn.onClick.AddListener(OnClick_invenBtn);

        obj = GameObject.Find("MenuBtn");
        menuBtn = obj.GetComponent<Button>();
        menuBtn.onClick.AddListener(OnClick_MenuBtn);

        obj = GameObject.Find("ProtraitBack");
        EXP = obj.GetComponent<Image>();
        EXP.fillAmount = GameManager.Inst.PlayerCurrentEXP / 400.0f;

        obj = GameObject.Find("Level");
        level = obj.GetComponent<TextMeshProUGUI>();
        level.text = "Lv. " + GameManager.Inst.PlayerLevel.ToString();

        obj = GameObject.Find("NickName");
        nickName = obj.GetComponent<TextMeshProUGUI>();
        nickName.text = GameManager.Inst.PlayerName.ToString();

        obj = GameObject.Find("HPBar");
        HP = obj.GetComponent<Image>();
        HP.fillAmount = 0.7f;

        obj = GameObject.Find("MPBar");
        MP = obj.GetComponent<Image>();
        MP.fillAmount = 0.8f;

        InitMenuManager();
    }

    public void InitMenuManager()
    {
        isOpenInventory = false;
        inventoryObj.LeanScale(Vector3.zero, 0.01f);
    }

    public void ShowInventory()
    {
        // 현재 상태의 반대로 전환
        isOpenInventory = !isOpenInventory;
        if(isOpenInventory == true)
        {
            // 아이템 정보를 최신으로 갱신
            inventoryUI.RefreshIcon();
            // 열기
            inventoryObj.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
        }
        else
        {
            // 닫기
            inventoryObj.LeanScale(Vector3.zero, 0.7f).setEase(LeanTweenType.easeInOutElastic);
        }
    }

    public void OnClick_invenBtn()
    {
        ShowInventory();
    }

    public void OnClick_MenuBtn()
    {

    }
}
