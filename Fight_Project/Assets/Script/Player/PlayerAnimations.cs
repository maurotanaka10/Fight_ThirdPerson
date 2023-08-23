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
        _animator.SetFloat(a_velocity, PlayerMovement.instance._currentVelocity);

        if(PlayerMovement.instance.JumpPressed && !isJumpingAnimation)
        {
            _animator.SetBool(a_isJumping, true);
        }
        else if(!PlayerMovement.instance.JumpPressed && isJumpingAnimation)
        {
            _animator.SetBool(a_isJumping, false);
        }
        else if(PlayerMovement.instance.JumpPressed && isJumpingAnimation)
        {
            _animator.SetBool(a_isJumping, false);
        }

        if(PlayerMovement.instance.IsGrounded && PlayerAttack.instance.IsAttacking && !firstAttackAnimation)
        {
            _animator.SetBool(a_firstAttack, true);
        }
        else if(PlayerMovement.instance.IsGrounded && PlayerAttack.instance.IsAttacking && firstAttackAnimation)
        {
            _animator.SetBool(a_firstAttack, false);
        }
        else if (PlayerMovement.instance.IsGrounded && !PlayerAttack.instance.IsAttacking && !firstAttackAnimation)
        {
            _animator.SetBool(a_firstAttack, false);
        }
        else if (PlayerMovement.instance.IsGrounded && !PlayerAttack.instance.IsAttacking && firstAttackAnimation)
        {
            _animator.SetBool(a_firstAttack, false);
        }
    }
}
