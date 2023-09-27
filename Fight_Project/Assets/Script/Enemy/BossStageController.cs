using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageController : MonoBehaviour
{
    [SerializeField] private GameObject[] bossDoor;

    private void Awake()
    {
        bossDoor[0].SetActive(true);
        bossDoor[1].SetActive(true);

        GameManager.OnNumberOfKillsReceived += BossStateHandler;
    }

    private void BossStateHandler(int obj)
    {
        if (obj == 5)
        {
            bossDoor[0].SetActive(false);
            bossDoor[1].SetActive(false);
        }
    }
}
