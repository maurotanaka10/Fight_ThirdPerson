using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SphereCollider))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private BossController _bossController;
    private SphereCollider _sphereCollider;

    public event Action<bool, int> OnHitEnemy;

    private bool _hitEnemy;
    private int numberOfKills;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.enabled = false;

        PlayerManager.HandleAttackInput += AttackHandler;
    }

    private void Update()
    {
        _hitEnemy = false;
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
            _hitEnemy = true;
            numberOfKills++;
            OnHitEnemy?.Invoke(_hitEnemy, numberOfKills);
        }

        if (collider.gameObject.CompareTag("Boss"))
        {
            _bossController.LifeBoss--;
            print($"Acertou o boss");
        }
    }
}