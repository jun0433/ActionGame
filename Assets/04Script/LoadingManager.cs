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
            Debug.Log("LoadingManager.cs - Awake() - tipText 참조 실패");
        }

        obj = GameObject.Find("Fill");
        if (obj != null)
        {
            fillImage = obj.GetComponent<Image>();
        }
        else
        {
            Debug.Log("LoadingManager.cs - Awake() - fillImage 참조 실패");
        }

        StartCoroutine("LoadAsyncScene");
    }


    IEnumerator LoadAsyncScene()
    {
        yield return null;
        tipText.text = "test";

        yield return new WaitForSeconds(2f); // 캐싱해서 사용해야함.

        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(GameManager.Inst.NextScene.ToString());
        asyncScene.allowSceneActivation = false; // 자동씬 변경 방지

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
                    asyncScene.allowSceneActivation = true; // 다음씬으로 전환
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
