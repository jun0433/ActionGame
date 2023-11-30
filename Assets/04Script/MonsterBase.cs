using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    private MonsterAI monsterAI;

    private void Awake()
    {
        monsterAI = GetComponent<MonsterAI>();

        monsterAI.StartAI();
    }
}
