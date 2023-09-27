using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject[] _spawnPosition;
    
    private void Awake()
    {
        GameManager.OnNumberOfKillsReceived += SpawnEnemyHandler;
    }

    private void SpawnEnemyHandler(int obj)
    {
        if (obj == 1)
        {
            StartCoroutine(TimerToInstantiateEnemy());
            Instantiate(_enemyPrefab, _spawnPosition[0].transform.position, _spawnPosition[0].transform.rotation);
        }
        else if (obj == 2)
        {
            StartCoroutine(TimerToInstantiateEnemy());
            Instantiate(_enemyPrefab, _spawnPosition[1].transform.position, _spawnPosition[1].transform.rotation);
        }
        else if (obj == 3)
        {
            StartCoroutine(TimerToInstantiateEnemy());
            Instantiate(_enemyPrefab, _spawnPosition[2].transform.position, _spawnPosition[2].transform.rotation);
        }
        else if (obj == 4)
        {
            StartCoroutine(TimerToInstantiateEnemy());
            Instantiate(_enemyPrefab, _spawnPosition[3].transform.position, _spawnPosition[3].transform.rotation);
        }
    }

    private IEnumerator TimerToInstantiateEnemy()
    {
        yield return new WaitForSeconds(5f);
    }
}
