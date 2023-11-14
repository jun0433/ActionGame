using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    private TextMeshProUGUI tipText;
    private Image fillImage;
    private GameObject obj;

    private void Awake()
    {
        obj = GameObject.Find("TipText");
        if(obj != null)
        {
            tipText = obj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.Log("LoadingManager.cs - Awake() - tipText ���� ����");
        }

        obj = GameObject.Find("Fill");
        if (obj != null)
        {
            fillImage = obj.GetComponent<Image>();
        }
        else
        {
            Debug.Log("LoadingManager.cs - Awake() - fillImage ���� ����");
        }

        StartCoroutine("LoadAsyncScene");
    }


    IEnumerator LoadAsyncScene()
    {
        yield return null;
        tipText.text = "test";

        yield return new WaitForSeconds(2f); // ĳ���ؼ� ����ؾ���.

        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(GameManager.Inst.NextScene.ToString());
        asyncScene.allowSceneActivation = false; // �ڵ��� ���� ����

        float timeC = 0f;

        while (!asyncScene.isDone)
        {
            yield return null;
            timeC += Time.deltaTime;

            if(asyncScene.progress >= 0.9f)
            {
                fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, 1f, timeC);
                if(fillImage.fillAmount >= 0.99f)
                {
                    asyncScene.allowSceneActivation = true; // ���������� ��ȯ
                }
            }
            else
            {
                fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, asyncScene.progress, timeC);
                if(fillImage.fillAmount >= asyncScene.progress)
                {
                    timeC = 0f;
                }
            }
        }
    }
}
