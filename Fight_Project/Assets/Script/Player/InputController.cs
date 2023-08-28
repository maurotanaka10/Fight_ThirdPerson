using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private PlayerInputSystem _playerInputSystem;
    public static InputController Instance;

    private Vector2 _movementCharacterInput;
    public Vector3 MovementCharacter;

    public delegate void OnAttack(InputAction.CallbackContext context);
    public static event OnAttack onAttack;

    public delegate void OnJump(InputAction.CallbackContext context);
    public static event OnJump onJump;

    private void Awake()
    {
        _playerInputSystem = new PlayerInputSystem();

        _playerInputSystem.Player.Walk.started += OnMovementInput;
        _playerInputSystem.Player.Walk.canceled += OnMovementInput;
        _playerInputSystem.Player.Walk.performed += OnMovementInput;

        _playerInputSystem.Player.Jump.started += OnJumpInput;
        _playerInputSystem.Player.Jump.canceled += OnJumpInput;

        _playerInputSystem.Player.Attack.started += OnAttacking;
        _playerInputSystem.Player.Attack.canceled += OnAttacking;
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _movementCharacterInput = context.ReadValue<Vector2>();
        MovementCharacter = new Vector3(_movementCharacterInput.x, MovementCharacter.y, _movementCharacterInput.y);
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        onJump.Invoke(context);
    }
    private void OnAttacking(InputAction.CallbackContext context)
    {
        onAttack.Invoke(context);
    }

    private void OnEnable()
    {
        _playerInputSystem.Enable();
    }
    private void OnDisable()
    {
        _playerInputSystem.Disable();
    }
}
