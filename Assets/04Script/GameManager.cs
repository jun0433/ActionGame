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
    public int curHP;
    public int maxHP;
    public int curMP;
    public int maxMP;
    public int gold;
    public int uidCounter;
    public Inventory inventory;
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
        base.Awake();
        dataPath = Application.persistentDataPath + "/save";
        pData = new PlayerData();

        #region _TableData_
        table = Resources.Load<ActionGame>("ActionGame");

        // 리스트로 되어 있는 ItemData를 빠른 탐색을 위해 Dictionary로 변경 작업
        for(int i = 0; i< table.ItemData.Count; i++)
        {
            dicItemData.Add(table.ItemData[i].id, table.ItemData[i]);
        }

        #endregion

        UpdateGMinfo();
    }

    #region _TableData_
    private ActionGame table;
    private Dictionary<int, ItemData_Entity> dicItemData = new Dictionary<int, ItemData_Entity>();


    // 콜바이 벨류
    public bool GetItemData(int ItemID, out ItemData_Entity data)
    {
        return dicItemData.TryGetValue(ItemID, out data);
    }


    #endregion


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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            for(int i = 0; i < pData.inventory.MAXITEMCOUNT; i++)
            {
                Debug.Log(i + "번째 아이템은" + pData.inventory.GetItemList()[i].itemID + " 를 " + pData.inventory.GetItemList()[i].amount + "개 가지고 있습니다.");
            }
            
        }
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
        Debug.Log("씬 로딩 매니저" + nextLoadSceneName.ToString());

        // 페이드 인 아웃 작업
        // 세이브 생성 작업

        SceneManager.LoadScene(SceneName.LoadingScene.ToString());
    }

    #endregion

    #region _UserData_
    public void CreateUserData(string newNickName)
    {
        pData.userNickName= newNickName;
        pData.curEXP = 0;
        pData.level = 1;
        pData.gold = 5000;
        pData.curHP = pData.maxHP = 500;
        pData.curMP = pData.maxMP = 100;
        pData.uidCounter = 0;
    }
    #endregion


    #region _Items_
    // 아이템 습득처리하는 함수
    public bool LootingItem(InventoryitemData newItem)
    {
        if (!pData.inventory.IsFull())
        {
            pData.inventory.AddItem(newItem);
            return true;
        }
        return false;
    }

    public void DeleteItem(InventoryitemData deleteItem)
    {
        pData.inventory.DeleteItem(deleteItem);
    }
    #endregion


    #region _PlayerDataRead_
    public int PlayerGold
    {
        get => pData.gold;
        set => pData.gold = value;
    }

    public string PlayerName
    {
        get => pData.userNickName;
    }

    public int PlayerLevel
    {
        get => pData.level;
    }

    public int PlayerCurrentEXP
    {
        get => pData.curEXP;
    }

    public int PlayerUID
    {
        get
        {
            return ++pData.uidCounter;
        }
    }

    public Inventory Inven
    {
        get => pData.inventory;
    }

    #endregion
}