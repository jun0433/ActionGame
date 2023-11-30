using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScanner : MonoBehaviour
{
    private MonsterAI monsterAI;

    private void Awake()
    {
        monsterAI = transform.parent.GetComponent<MonsterAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && monsterAI != null)
        {
            monsterAI.SetTarget(other.gameObject);
        }
    }
}
