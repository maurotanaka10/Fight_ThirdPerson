using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private BossController _bossController;
    [SerializeField] private PlayerAttack _playerAttack;

[SerializeField] private GameObject winUI;
[SerializeField] private GameObject loseUI;
    [SerializeField] private TMP_Text killCountText;
    [SerializeField] private GameObject[] lifeImage;

    private int _killsCount;

    private void Awake()
    {
        _playerAttack.OnKillsEnemy += EnemyKillsCount;
    }

    private void Update()
    {
        if (_playerManager.PlayerLives == 0)
        {
            loseUI.SetActive(true);
            lifeImage[2].SetActive(false);
        }
        else if (_playerManager.PlayerLives == 2)
        {
            lifeImage[0].SetActive(false);
        }
        else if (_playerManager.PlayerLives == 1)
        {
            lifeImage[1].SetActive(false);
        }

        if (_bossController.LifeBoss == 0)
        {
            winUI.SetActive(true);
        }

        killCountText.text = _killsCount.ToString();
    }

    private void EnemyKillsCount(int killsCount)
    {
        this._killsCount = killsCount;
    }

    public void ResetButton()
    {
        SceneManager.LoadScene("Game");
    }
}
