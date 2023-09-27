using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SphereCollider))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private BossController _bossController;
    private StandardEnemyController _standardEnemyController;
    private SphereCollider _sphereCollider;

    public event Action<int> OnKillsEnemy;
    
    private int numberOfKills;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.enabled = false;

        PlayerManager.HandleAttackInput += AttackHandler;
    }

    private void AttackHandler(bool isAttacking)
    {
        if (!isAttacking) return;
        _sphereCollider.enabled = isAttacking;
        StartCoroutine(TurnAttackDetectionOff());
    }

    private IEnumerator TurnAttackDetectionOff()
    {
        yield return new WaitForSeconds(0.5f);
        _sphereCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            _standardEnemyController = FindObjectOfType<StandardEnemyController>();
            
            numberOfKills++;
            _standardEnemyController.EnemyStandardIsDead = true;
            OnKillsEnemy?.Invoke(numberOfKills);
        }

        if (collider.gameObject.CompareTag("Boss"))
        {
            _bossController.LifeBoss--;
            print($"Acertou o boss");
        }
    }
}