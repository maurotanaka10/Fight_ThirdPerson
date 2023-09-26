using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossManager : MonoBehaviour
{
    [SerializeField] private BossController _bossController;

    public static Action<EBossStates> OnIdleBossReceived;
    public static Action<EBossStates> OnPatrolBossReceived;
    public static Action<EBossStates> OnChaseBossReceived;
    public static Action<EBossStates> OnAttackNormalBossReceived;
    public static Action<EBossStates> OnAttack360BossReceived;
    public static Action<EBossStates> OnDieBossReceived;

    private void Awake()
    {
        _bossController.OnIdleBoss += OnIdleStatusReceived;
        _bossController.OnPatrolBoss += OnPatrolStatusReceived;
        _bossController.OnChaseBoss += OnChaseStatusReceived;
        _bossController.OnAttackNormalBoss += OnAttackNormalStatusReceived;
        _bossController.OnAttack360Boss += OnAttack360StatusReceived;
        _bossController.OnDieBoss += OnDieStatusReceived;
        
    }

    private void OnIdleStatusReceived(EBossStates obj)
    {
        OnIdleBossReceived?.Invoke(obj);
    }

    private void OnPatrolStatusReceived(EBossStates obj)
    {
        OnPatrolBossReceived?.Invoke(obj);
    }

    private void OnChaseStatusReceived(EBossStates obj)
    {
        OnChaseBossReceived?.Invoke(obj);
    }

    private void OnAttackNormalStatusReceived(EBossStates obj)
    {
        OnAttackNormalBossReceived?.Invoke(obj);
    }

    private void OnAttack360StatusReceived(EBossStates obj)
    {
        OnAttack360BossReceived?.Invoke(obj);
    }

    private void OnDieStatusReceived(EBossStates obj)
    {
        OnDieBossReceived?.Invoke(obj);
    }

    private void OnDisable()
    {
        _bossController.OnIdleBoss -= OnIdleStatusReceived;
        _bossController.OnPatrolBoss -= OnPatrolStatusReceived;
        _bossController.OnChaseBoss -= OnChaseStatusReceived;
        _bossController.OnAttackNormalBoss -= OnAttackNormalStatusReceived;
        _bossController.OnAttack360Boss -= OnAttack360StatusReceived;
        _bossController.OnDieBoss -= OnDieStatusReceived;
    }
}
