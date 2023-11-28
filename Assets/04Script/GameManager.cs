using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;


// 1. ���� �����ϴ� �۾�
// 2. �÷��̾� ���̺� ������ ����


public enum SceneName
{
    IntroScene,
    LoadingScene,
    TitleScene,
    Shrine,


}

[System.Serializable] // ����ȭ -> �ν�����â�� Ȯ�� �Ұ����� Ŀ���� class�� �ν�����â���� Ȯ�� �����ϵ��� ����

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

        // ����Ʈ�� �Ǿ� �ִ� ItemData�� ���� Ž���� ���� Dictionary�� ���� �۾�
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


    // �ݹ��� ����
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

    // ���ӸŴ����� �����͸� �����ؾ� �ϴ� ��Ȳ�� ȣ��(�� �ε� �� ���)
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
                Debug.Log(i + "��° ��������" + pData.inventory.GetItemList()[i].itemID + " �� " + pData.inventory.GetItemList()[i].amount + "�� ������ �ֽ��ϴ�.");
            }
            
        }
    }

    #region Save&Load
    // �ü��(������, �ȵ���̵�, IOS, PS)
    private string dataPath;

    public void SaveData()
    {
        // json ����
        string data = JsonUtility.ToJson(pData); // class ��ü�� string���� ��ȯ ����.
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

    // ���� �ٲ� �� ȣ���ϴ� �Լ�.(�ε��������� �� ����)
    public void AsyncLoadNextScene(SceneName nextName)
    {
        nextLoadSceneName = nextName;
        Debug.Log("�� �ε� �Ŵ���" + nextLoadSceneName.ToString());

        // ���̵� �� �ƿ� �۾�
        // ���̺� ���� �۾�

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
    // ������ ����ó���ϴ� �Լ�
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