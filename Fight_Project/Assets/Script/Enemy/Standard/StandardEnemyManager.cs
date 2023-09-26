using System;
using UnityEngine;


public class StandardEnemyManager : MonoBehaviour
{
    [SerializeField] private StandardEnemyController _standardEnemyController;
    
    public static Action<EStandardStates> OnIdleStatusReceived;
    public static Action<EStandardStates> OnPatrolStatusReceived;
    public static Action<EStandardStates> OnChaseStatusReceived;
    public static Action<EStandardStates> OnAttackStatusReceived;
    public static Action<EStandardStates> OnDieStatusReceived;

    public static Action<bool> OnHitEnemyStandardReceived;

    private void Awake()
    {
        _standardEnemyController.OnIdle += OnIdleReceived;
        _standardEnemyController.OnPatrol += OnPatrolReceived;
        _standardEnemyController.OnChase += OnChaseReceived;
        _standardEnemyController.OnDie += OnDieReceived;
        GameManager.OnHitEnemyColliderReceived += OnHitEnemyReceived;
    }

    private void OnHitEnemyReceived(bool hitEnemy)
    {
        OnHitEnemyStandardReceived?.Invoke(hitEnemy);
    }

    private void OnIdleReceived(EStandardStates currentState)
    {
        OnIdleStatusReceived?.Invoke(currentState);
    }

    private void OnPatrolReceived(EStandardStates currentState)
    {
        OnPatrolStatusReceived?.Invoke(currentState);
    }

    private void OnChaseReceived(EStandardStates currentState)
    {
        OnChaseStatusReceived?.Invoke(currentState);
    }

    private void OnAttackReceived(EStandardStates currentState)
    {
        OnAttackStatusReceived?.Invoke(currentState);
    }

    private void OnDieReceived(EStandardStates currentState)
    {
        OnDieStatusReceived?.Invoke(currentState);
    }

    private void OnDisable()
    {
        _standardEnemyController.OnIdle -= OnIdleReceived;
        _standardEnemyController.OnPatrol -= OnPatrolReceived;
        _standardEnemyController.OnChase -= OnChaseReceived;
        _standardEnemyController.OnDie -= OnDieReceived;
    }
}
