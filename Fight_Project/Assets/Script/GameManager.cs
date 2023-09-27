using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private PlayerManager _playerManager;
    
    public static Action<InputAction.CallbackContext> OnMoveInputContextReceived;
    public static Action<bool> OnJumpInputContextReceived;
    public static Action<bool> OnAttackInputContextReceived;
    public static Action<int> OnNumberOfKillsReceived;

    private void Awake()
    {
        _inputManager.OnMove += OnMoveInputReceived;
        _inputManager.OnJump += OnJumpInputReceived;
        _inputManager.OnAttack += OnAttackInputReceived;
        _playerManager.HandleKillsEnemy += OnKillsEnemyReceived;
    }

    private void OnKillsEnemyReceived(int numberOfKills)
    {
        OnNumberOfKillsReceived?.Invoke(numberOfKills);
    }

    private void OnMoveInputReceived(InputAction.CallbackContext context)
    {
        OnMoveInputContextReceived?.Invoke(context);
    }

    private void OnJumpInputReceived(bool isJumpPressed)
    {
        OnJumpInputContextReceived?.Invoke(isJumpPressed);
    }

    private void OnAttackInputReceived(bool isAttackPressed)
    {
        OnAttackInputContextReceived?.Invoke(isAttackPressed);
    }

    private void OnDisable()
    {
        _inputManager.OnMove -= OnMoveInputReceived;
        _inputManager.OnJump -= OnJumpInputReceived;
        _inputManager.OnAttack -= OnAttackInputReceived;
    }
}
