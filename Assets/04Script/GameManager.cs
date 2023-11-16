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

    // ���ӸŴ����� �����͸� �����ؾ� �ϴ� ��Ȳ�� ȣ��(�� �ε� �� ���)
    public void UpdateGMinfo()
    {
        LoadData();
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

        // ���̵� �� �ƿ� �۾�
        // ���̺� ���� �۾�

        SceneManager.LoadScene(SceneName.LoadingScene.ToString());
    }

    #endregion

}