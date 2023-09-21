using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInputSystem _playerInputSystem;

    public event Action<InputAction.CallbackContext> OnMove;
    public event Action<bool> OnJump;
    public event Action<bool> OnAttack;
    
    private void Awake()
    {
        _playerInputSystem = new PlayerInputSystem();

        _playerInputSystem.Player.Walk.started += OnMovementInput;
        _playerInputSystem.Player.Walk.canceled += OnMovementInput;
        _playerInputSystem.Player.Walk.performed += OnMovementInput;

        _playerInputSystem.Player.Jump.started += OnJumpInput;
        _playerInputSystem.Player.Jump.canceled += OnJumpInput;

        _playerInputSystem.Player.Attack.started += OnAttackInput;
        _playerInputSystem.Player.Attack.canceled += OnAttackInput;
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(context);
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        OnJump?.Invoke(context.ReadValueAsButton());
    }
    private void OnAttackInput(InputAction.CallbackContext context)
    {
        OnAttack?.Invoke(context.ReadValueAsButton());
    }

    private void OnEnable()
    {
        _playerInputSystem.Enable();
    }
    private void OnDisable()
    {
        _playerInputSystem.Disable();
        
        _playerInputSystem.Player.Walk.started -= OnMovementInput;
        _playerInputSystem.Player.Walk.canceled -= OnMovementInput;
        _playerInputSystem.Player.Walk.performed -= OnMovementInput;

        _playerInputSystem.Player.Jump.started -= OnJumpInput;
        _playerInputSystem.Player.Jump.canceled -= OnJumpInput;

        _playerInputSystem.Player.Attack.started -= OnAttackInput;
        _playerInputSystem.Player.Attack.canceled -= OnAttackInput;
    }
}
