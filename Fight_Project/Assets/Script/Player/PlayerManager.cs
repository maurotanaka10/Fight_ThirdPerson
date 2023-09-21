using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static event Action<InputAction.CallbackContext, float> HandleMoveInput;
    public static event Action<bool, int> HandleJumpInput;
    public static event Action<bool> HandleAttackInput;

    public delegate CharacterController CharacterControllerReference();

    public static CharacterControllerReference _characterControllerReference;

    private int _numberOfJumps = 0;
    
    [SerializeField] private float _velocity = 10;
    [SerializeField] private int _lives = 1;

    private void Awake()
    {
        PlayerManagerSetUpListeners();
    }

    private void PlayerManagerSetUpListeners()
    {
        GameManager.OnMoveInputContextReceived += HandleMove;
        GameManager.OnJumpInputContextReceived += HandleJump;
        GameManager.OnAttackInputContextReceived += HandleAttack;
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        HandleMoveInput?.Invoke(context, _velocity);
    }

    private void HandleJump(bool isJumpPressed)
    {
        CharacterController _tempController = _characterControllerReference?.Invoke();
        if (_tempController.isGrounded == true) _numberOfJumps = 0;
        if (isJumpPressed) _numberOfJumps++;
        HandleJumpInput?.Invoke(isJumpPressed, _numberOfJumps);
    }

    private void HandleAttack(bool isAttackPressed)
    {
        HandleAttackInput?.Invoke(isAttackPressed);
    }

    private void OnDisable()
    {
        GameManager.OnMoveInputContextReceived -= HandleMove;
        GameManager.OnJumpInputContextReceived -= HandleJump;
        GameManager.OnAttackInputContextReceived -= HandleAttack;
    }
}