using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;

    private int _isJumpingHash;
    private int _velocityHash;
    private int _isAttackHash;
    private int _numberOfJumpsHash;

    private int _numberOfJumps;
    private bool _isJumping;
    private bool _isAttacking;
    private float _currentVelocity = 0;

    private void Awake()
    {
        GetAnimatorParameters();
        _animator = GetComponent<Animator>();

        PlayerManager.HandleJumpInput += JumpAnimationHandler;
        PlayerManager.HandleAttackInput += AttackAnimationHandler;
    }

    private void GetAnimatorParameters()
    {
        _isJumpingHash = Animator.StringToHash("isJumping");
        _velocityHash = Animator.StringToHash("velocity");
        _isAttackHash = Animator.StringToHash("isAttacking");
        _numberOfJumpsHash = Animator.StringToHash("numberOfJumps");
    }

    private void Update()
    {
        MoveAnimationHandler();
    }

    private void MoveAnimationHandler()
    {
        CharacterController _tempController = PlayerManager._characterControllerReference?.Invoke();
        _currentVelocity = _tempController.velocity.magnitude;

        _animator.SetFloat(_velocityHash, _currentVelocity);
    }

    private void JumpAnimationHandler(bool isJumpPressed, int numberOfJumps)
    {
        CharacterController _tempController = PlayerManager._characterControllerReference?.Invoke();
        if (isJumpPressed) this._numberOfJumps = numberOfJumps;
        _isJumping = isJumpPressed;

        bool _isJumpingAnimation = _animator.GetBool(_isJumpingHash);
        
        _animator.SetInteger(_numberOfJumpsHash, _numberOfJumps);

        if (_tempController.isGrounded || _animator.GetInteger(numberOfJumps) > 2)
        {
            _animator.SetInteger(numberOfJumps, 0);
        }
        if (_isJumping && !_isJumpingAnimation && _tempController.isGrounded)
        {
            _animator.SetBool(_isJumpingHash, true);
        }
        else if (_isJumpingAnimation || _tempController.isGrounded)
        {
            _animator.SetBool(_isJumpingHash, false);
        }
    }
    private void AttackAnimationHandler(bool isAttackPressed)
    {
        this._isAttacking = isAttackPressed;
        if (this._isAttacking && !_animator.GetBool(_isAttackHash))
        {
            _animator.SetBool(_isAttackHash, true);
        }
        else if (_animator.GetBool(_isAttackHash) && !this._isAttacking)
        {
            _animator.SetBool(_isAttackHash, false);
        }
    }

    private void OnDisable()
    {
        PlayerManager.HandleJumpInput -= JumpAnimationHandler;
        PlayerManager.HandleAttackInput -= AttackAnimationHandler;
    }
}