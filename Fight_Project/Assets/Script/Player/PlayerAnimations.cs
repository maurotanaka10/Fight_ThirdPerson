using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;

    private int a_isJumping;
    private int a_velocity;
    private int a_attack1;
    private int a_attack2;
    private int a_attack3;

    private void Awake()
    {
        GetAnimatorParameters();
        _animator = GetComponent<Animator>();
    }

    private void GetAnimatorParameters()
    {
        a_isJumping = Animator.StringToHash("isJumping");
        a_velocity = Animator.StringToHash("velocity");
        a_attack1 = Animator.StringToHash("attack1");
        a_attack2 = Animator.StringToHash("attack2");
        a_attack3 = Animator.StringToHash("attack3");
    }

    private void Update()
    {
        AnimationHandler();
    }

    private void AnimationHandler()
    {
        bool isJumpingAnimation = _animator.GetBool(a_isJumping);
        bool attack1Animation = _animator.GetBool(a_attack1);
        bool attack2Animation = _animator.GetBool(a_attack2);
        bool attack3Animation = _animator.GetBool(a_attack3);

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
        if(PlayerManager.Instance.GetIsGrounded() && PlayerManager.Instance.GetPlayerAttacking() && !attack1Animation)
        {
            _animator.SetBool(a_attack1, true);
        }
        else if(PlayerManager.Instance.GetIsGrounded() && PlayerManager.Instance.GetPlayerAttacking() && attack1Animation)
        {
            _animator.SetBool(a_attack1, false);
        }
        else if (PlayerManager.Instance.GetIsGrounded() && !PlayerManager.Instance.GetPlayerAttacking() && attack1Animation)
        {
            _animator.SetBool(a_attack1, false);
        }
        #endregion

        #region Second Animation Requirement
        if (PlayerManager.Instance.GetIsGrounded() && PlayerManager.Instance.GetPlayerAttacking() && !attack2Animation && PlayerManager.Instance.GetAttackComboIndex() == 3)
        {
            _animator.SetBool(a_attack2, true);
        }
        else if (PlayerManager.Instance.GetIsGrounded() && PlayerManager.Instance.GetPlayerAttacking() && attack2Animation)
        {
            _animator.SetBool(a_attack2, false);
        }
        else if (PlayerManager.Instance.GetIsGrounded() && !PlayerManager.Instance.GetPlayerAttacking() && attack2Animation)
        {
            _animator.SetBool(a_attack2, false);
        }
        #endregion

        #region Second Animation Requirement

        #endregion
    }
}

