using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: MonoBehaviour {

    private static T instance;
    public static T Inst {
        get
        {
            if(instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    public void Awake()
    {
        // 부모 오브젝트가 존재하는 경우
        if(transform.parent != null && transform.root != null)
        {
            DontDestroyOnLoad(this.transform.root.gameObject); // 최상위 부모를 씬이 변경되더라도 삭제 되지 않도록,
        }
        else // 자기 자신이 root인 경우
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}


