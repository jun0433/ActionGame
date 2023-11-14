using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;


// 1. 씬을 변경하는 작업
// 2. 플레이어 세이브 데이터 관리


public enum SceneName
{
    IntroScene,
    LoadingScene,
    TitleScene,
    Shrine,


}

[System.Serializable] // 직렬화 -> 인스펙터창에 확인 불가능한 커스텀 class를 인스펙터창에서 확인 가능하도록 변경

public class PlayerData
{
    public string userNickName;
    public int level;
    public int curEXP;
    public int maxHP;
    public int curMP;
    public int maxMP;
    public int gold;
    public int uidCounter;
}

public class GameManager : Singleton<GameManager>
{

    private PlayerData pData;

    public PlayerData PlayerInfo
    {
        get => pData;
    }




    private void Awake()
    {
        dataPath = Application.persistentDataPath + "/save";
        pData = new PlayerData();
        SaveData();
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level != 0)
        {
            UpdateGMinfo();
        }

    }

    // 게임매니저의 데이터를 갱신해야 하는 상황에 호출(씬 로딩 등등에 사용)
    public void UpdateGMinfo()
    {
        LoadData();
    }

    #region Save&Load
    // 운영체제(윈도우, 안드로이드, IOS, PS)
    private string dataPath;

    public void SaveData()
    {
        // json 포맷
        string data = JsonUtility.ToJson(pData); // class 객체를 string으로 변환 저장.
        Debug.Log(data);
        File.WriteAllText(dataPath, data);
    }

    public bool LoadData()
    {
        if (File.Exists(dataPath))
        {
            string data = File.ReadAllText(dataPath);
            pData = JsonUtility.FromJson<PlayerData>(data);
            return true;
        }
        return false;
    }

    public void DeleteData()
    {
        File.Delete(dataPath);
    }
    #endregion

    #region LoadingSceneLogic
    private SceneName nextLoadSceneName;
    public SceneName NextScene
    {
        get => nextLoadSceneName;
    }

    // 씬을 바꿀 때 호출하는 함수.(로딩씬을통해 씬 변경)
    public void AsyncLoadNextScene(SceneName nextName)
    {
        nextLoadSceneName = nextName;

        // 페이드 인 아웃 작업
        // 세이브 생성 작업

        SceneManager.LoadScene(SceneName.LoadingScene.ToString());
    }

    #endregion

}