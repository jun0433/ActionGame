using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSceneManager : MonoBehaviour
{

    private GameObject logoObj;

    private void Awake()
    {
        logoObj = GameObject.Find("LogoImage");
        if(logoObj != null)
        {
            LeanTween.moveLocalY(logoObj, 0f, 3f).setEase(LeanTweenType.easeOutBounce);
            LeanTween.moveLocalX(logoObj, 0f, 3f).setEase(LeanTweenType.easeInSine);
            LeanTween.rotate(logoObj, Vector3.zero, 3f);
            Invoke("AutoNextScene", 4.5f);
        }
    }


    private void AutoNextScene()
    {
        //  gameManager�� ���ؼ� ���� ������ ����
        Debug.Log(GameManager.Inst.gameObject.name);
        GameManager.Inst.AsyncLoadNextScene(SceneName.TitleScene);
    }



}
