using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BossAnimationsComponent : MonoBehaviour
{
    private Animator _animator;

    private int _isIdleHash;
    private int _isPatrollingHash;
    private int _isChaseHash;
    private int _isAttackNormalHash;
    private int _isAttack360Hash;
    private int _isHitHash;
    private int _isDieHash;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        GetBossAnimatorParameters();

        BossManager.OnIdleBossReceived += IdleAnimationHandler;
        BossManager.OnPatrolBossReceived += PatrolAnimationHandler;
        BossManager.OnChaseBossReceived += ChaseAnimationHandler;
        BossManager.OnAttackNormalBossReceived += AttackNormalAnimationHandler;
        BossManager.OnAttack360BossReceived += Attack360AnimationHandler;
        BossManager.OnHitBossReceived += HitAnimationHandler;
        BossManager.OnDieBossReceived += DieAnimationHandler;
    }

    private void IdleAnimationHandler(EBossStates obj)
    {
        if(obj == EBossStates.Idle)
            _animator.SetTrigger(_isIdleHash);
    }

    private void PatrolAnimationHandler(EBossStates obj)
    {
        if(obj == EBossStates.Patrol)
            _animator.SetTrigger(_isPatrollingHash);
    }

    private void ChaseAnimationHandler(EBossStates obj)
    {
        if(obj == EBossStates.Chase)
            _animator.SetTrigger(_isChaseHash);
    }

    private void AttackNormalAnimationHandler(EBossStates obj)
    {
        if(obj == EBossStates.AttackNormal)
            _animator.SetTrigger(_isAttackNormalHash);
    }

    private void Attack360AnimationHandler(EBossStates obj)
    {
        if(obj == EBossStates.Attack360)
            _animator.SetTrigger(_isAttack360Hash);
    }

    private void HitAnimationHandler(EBossStates obj)
    {
        if(obj == EBossStates.Hit)
            _animator.SetTrigger(_isHitHash);
    }

    private void DieAnimationHandler(EBossStates obj)
    {
        if(obj == EBossStates.Die)
            _animator.SetTrigger(_isDieHash);
    }

    private void GetBossAnimatorParameters()
    {
        _isIdleHash = Animator.StringToHash("isIdle");
        _isPatrollingHash = Animator.StringToHash("isPatrolling");
        _isChaseHash = Animator.StringToHash("isChasing");
        _isAttackNormalHash = Animator.StringToHash("isAttackNormal");
        _isAttack360Hash = Animator.StringToHash("isAttack360");
        _isHitHash = Animator.StringToHash("getHit");
        _isDieHash = Animator.StringToHash("isDeath");
    }

    private void OnDisable()
    {
        BossManager.OnIdleBossReceived -= IdleAnimationHandler;
        BossManager.OnPatrolBossReceived -= PatrolAnimationHandler;
        BossManager.OnChaseBossReceived -= ChaseAnimationHandler;
        BossManager.OnAttackNormalBossReceived -= AttackNormalAnimationHandler;
        BossManager.OnAttack360BossReceived -= Attack360AnimationHandler;
        BossManager.OnHitBossReceived -= HitAnimationHandler;
        BossManager.OnDieBossReceived -= DieAnimationHandler;
    }
}
