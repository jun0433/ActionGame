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
        //createBtn.onClick.AddListener(delegate { OnClick_CreateBtn("안녕"); }); // 델리게이트
        //createBtn.onClick.AddListener(() => OnClick_CreateBtn("람다"));
        InitTitleScene();


    }

    public void InitTitleScene()
    {
        isHaveUserData = GameManager.Inst.LoadData();
        if (isHaveUserData)
        {
            enterText.text = GameManager.Inst.PlayerInfo.userNickName + " 님 환영합니다.\n";
        }
        else
        {
            enterText.text = "계정을 생성하려면 터치하세요.\n";
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
        WarningText("계정 데이터가 모두 초기화 되었습니다.");
        GameManager.Inst.DeleteData(); // 데이터 삭제
        InitTitleScene(); // 화면 갱신
    }
    public void OnClick_CreateBtn()
    {
        // 계정 생성 성공
        if(newNickName.Length >= 2)
        {
            LeanTween.scale(nickNamePopup, Vector3.zero, 0.7f).setEase(LeanTweenType.easeInOutElastic);
            enterText.enabled = true;
            // 게임매니저에서 신규 계정을 생성
            GameManager.Inst.CreateUserData(newNickName);
            GameManager.Inst.SaveData();
            InitTitleScene();
        }
        // 실패
        else
        {
            WarningText("닉네임의 형식이 올바르지 않습니다.");
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
