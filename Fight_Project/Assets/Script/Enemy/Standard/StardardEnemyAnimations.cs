using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StardardEnemyAnimations : MonoBehaviour
{
    private Animator _animator;

    private int _isIdleHash;
    private int _isPatrolHash;
    private int _isChaseHash;
    private int _isDieHash;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        GetAnimatorParameters();

        StandardEnemyManager.OnIdleStatusReceived += IdleAnimationHandler;
        StandardEnemyManager.OnPatrolStatusReceived += PatrolAnimationHandler;
        StandardEnemyManager.OnChaseStatusReceived += ChaseAnimationHandler;
        StandardEnemyManager.OnDieStatusReceived += DieAnimationHandler;
    }

    private void IdleAnimationHandler(EStandardStates currentState)
    {
        if (currentState == EStandardStates.Idle)
            _animator.SetTrigger(_isIdleHash);
    }

    private void PatrolAnimationHandler(EStandardStates currentState)
    {
        if (currentState == EStandardStates.Patrol)
            _animator.SetTrigger(_isPatrolHash);
    }

    private void ChaseAnimationHandler(EStandardStates currentState)
    {
        if (currentState == EStandardStates.Chase)
            _animator.SetTrigger(_isChaseHash);
    }

    private void DieAnimationHandler(EStandardStates currentState)
    {
        if (currentState == EStandardStates.Die)
            _animator.SetTrigger(_isDieHash);
    }

    private void GetAnimatorParameters()
    {
        _isIdleHash = Animator.StringToHash("isIdle");
        _isPatrolHash = Animator.StringToHash("isPatrolling");
        _isChaseHash = Animator.StringToHash("isChasing");
        _isDieHash = Animator.StringToHash("isDeath");
    }

    private void OnDisable()
    {
        StandardEnemyManager.OnIdleStatusReceived -= IdleAnimationHandler;
        StandardEnemyManager.OnPatrolStatusReceived -= PatrolAnimationHandler;
        StandardEnemyManager.OnChaseStatusReceived -= ChaseAnimationHandler;
        StandardEnemyManager.OnDieStatusReceived -= DieAnimationHandler;
    }
}