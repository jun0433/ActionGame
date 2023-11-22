using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviour
{
    private TextMeshProUGUI enterText;
    private TextMeshProUGUI warningText;
    private GameObject nickNamePopup;
    private Button enterBtn;
    private Button deleteBtn;
    private Button createBtn;

    private GameObject obj;
    private bool isHaveUserData;

    private void Awake()
    {
        obj = GameObject.Find("EnterText");
        enterText = obj.GetComponent<TextMeshProUGUI>();
        obj = GameObject.Find("WarningText");
        warningText = obj.GetComponent<TextMeshProUGUI>();

        nickNamePopup = GameObject.Find("CreateUserPopup");

        obj = GameObject.Find("EnterBtn");
        enterBtn = obj.GetComponent<Button>();
        enterBtn.onClick.AddListener(OnClick_EnterBtn);

        obj = GameObject.Find("DeleteBtn");
        deleteBtn = obj.GetComponent<Button>();
        deleteBtn.onClick.AddListener(OnClick_DeleteBtn);

        obj = GameObject.Find("CreateBtn");
        createBtn = obj.GetComponent<Button>();
        createBtn.onClick.AddListener(OnClick_CreateBtn);
        //createBtn.onClick.AddListener(delegate { OnClick_CreateBtn("�ȳ�"); }); // ��������Ʈ
        //createBtn.onClick.AddListener(() => OnClick_CreateBtn("����"));
        InitTitleScene();


    }

    public void InitTitleScene()
    {
        isHaveUserData = GameManager.Inst.LoadData();
        if (isHaveUserData)
        {
            enterText.text = GameManager.Inst.PlayerInfo.userNickName + " �� ȯ���մϴ�.\n";
        }
        else
        {
            enterText.text = "������ �����Ϸ��� ��ġ�ϼ���.\n";
        }
    }

    public void OnClick_EnterBtn()
    {
        if (isHaveUserData)
        {
            GameManager.Inst.AsyncLoadNextScene(SceneName.Shrine);
        }
        else
        {
            LeanTween.scale(nickNamePopup, Vector3.one, 0.7f).setEase(LeanTweenType.easeOutElastic);
            enterText.enabled = false;
        }
    }

    public void OnClick_DeleteBtn()
    {
        WarningText("���� �����Ͱ� ��� �ʱ�ȭ �Ǿ����ϴ�.");
        GameManager.Inst.DeleteData(); // ������ ����
        InitTitleScene(); // ȭ�� ����
    }
    public void OnClick_CreateBtn()
    {
        // ���� ���� ����
        if(newNickName.Length >= 2)
        {
            LeanTween.scale(nickNamePopup, Vector3.zero, 0.7f).setEase(LeanTweenType.easeInOutElastic);
            enterText.enabled = true;
            // ���ӸŴ������� �ű� ������ ����
            GameManager.Inst.CreateUserData(newNickName);
            GameManager.Inst.SaveData();
            InitTitleScene();
        }
        // ����
        else
        {
            WarningText("�г����� ������ �ùٸ��� �ʽ��ϴ�.");
        }
    }

    private string newNickName = "";

    public void OnChanged_NewNickName(string input)
    {
        newNickName = input;
    }

    void WarningText(string newMessage)
    {
        warningText.text = newMessage;
        Color fromColor = Color.red;
        fromColor.a = 0f;
        Color toColor = Color.red;
        toColor.a = 1f;

        LeanTween.value(warningText.gameObject, UpdateColor, fromColor, toColor, 1f).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.value(warningText.gameObject, UpdateColor, toColor, fromColor, 1f).setEase(LeanTweenType.easeInOutQuad).setDelay(1f);
    }

    private void UpdateColor(Color value)
    {
        warningText.color = value;
    }
}
