using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;

    private int a_isJumping;
    private int a_velocity;
    private int a_firstAttack;
    private int a_secondAttack;
    private int a_lastAttack;

    private void Awake()
    {
        GetAnimatorParameters();
        _animator = GetComponent<Animator>();
    }

    private void GetAnimatorParameters()
    {
        a_isJumping = Animator.StringToHash("isJumping");
        a_velocity = Animator.StringToHash("velocity");
        a_firstAttack = Animator.StringToHash("firstAttack");
        a_secondAttack = Animator.StringToHash("secondAttack");
        a_lastAttack = Animator.StringToHash("lastAttack");
    }

    private void Update()
    {
        AnimationHandler();
    }

    private void AnimationHandler()
    {
        bool isJumpingAnimation = _animator.GetBool(a_isJumping);
        bool firstAttackAnimation = _animator.GetBool(a_firstAttack);
        bool secondAttackAnimation = _animator.GetBool(a_secondAttack);
        bool lastAttackAnimation = _animator.GetBool(a_lastAttack);

        #region Movement Animation Requirement
        _animator.SetFloat(a_velocity, PlayerManager.Instance.GetCurrentVelocity());
        #endregion

        #region Jump Animation Requirement
        if (PlayerManager.Instance.GetJumpPressed() && !isJumpingAnimation)
        {
            _animator.SetBool(a_isJumping, true);
        }
        else if (!PlayerManager.Instance.GetJumpPressed() && isJumpingAnimation)
        {
            _animator.SetBool(a_isJumping, false);
        }
        else if (PlayerManager.Instance.GetJumpPressed() && isJumpingAnimation)
        {
            _animator.SetBool(a_isJumping, false);
        }
        #endregion

        #region First Attack Animation Requirement
        if (PlayerManager.Instance.GetIsGrounded() && PlayerManager.Instance.GetPlayerAttacking() && !firstAttackAnimation)
        {
            _animator.SetBool(a_firstAttack, true);
        }
        else if (PlayerManager.Instance.GetIsGrounded() && PlayerManager.Instance.GetPlayerAttacking() && firstAttackAnimation)
        {
            _animator.SetBool(a_firstAttack, false);
        }
        else if (PlayerManager.Instance.GetIsGrounded() && !PlayerManager.Instance.GetPlayerAttacking() && !firstAttackAnimation)
        {
            _animator.SetBool(a_firstAttack, false);
        }
        else if (PlayerManager.Instance.GetIsGrounded() && !PlayerManager.Instance.GetPlayerAttacking() && firstAttackAnimation)
        {
            _animator.SetBool(a_firstAttack, false);
        }
        #endregion

        #region Second Animation Requirement
        if (PlayerManager.Instance.GetIsGrounded() && PlayerManager.Instance.GetPlayerAttacking() && !secondAttackAnimation)
        {
            _animator.SetBool(a_secondAttack, true);
        }
        else if (PlayerManager.Instance.GetIsGrounded() && PlayerManager.Instance.GetPlayerAttacking() && secondAttackAnimation)
        {
            _animator.SetBool(a_secondAttack, false);
        }
        else if (PlayerManager.Instance.GetIsGrounded() && !PlayerManager.Instance.GetPlayerAttacking() && !secondAttackAnimation)
        {
            _animator.SetBool(a_secondAttack, false);
        }
        #endregion

    }
}

