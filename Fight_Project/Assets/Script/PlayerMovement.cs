using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;
    private PlayerInputSystem _playerInputSystem;

    private Vector2 _movementCharacterInput;
    private Vector3 _movementCharacter;
    private Vector3 _positionToLookAt;
    private float _verticalVelocity;

    public bool IsGrounded;
    public bool IsMoving;
    public bool JumpPressed;

    [SerializeField] private float _velocity;
    [SerializeField] private float _rotationVelocity;
    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private Transform _footPosition;
    [SerializeField] private LayerMask _groundLayer;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInputSystem = new PlayerInputSystem();

        _playerInputSystem.Player.Walk.started += OnMovementInput;
        _playerInputSystem.Player.Walk.canceled += OnMovementInput;
        _playerInputSystem.Player.Walk.performed += OnMovementInput;

        _playerInputSystem.Player.Jump.started += OnJumpInput;
        _playerInputSystem.Player.Jump.canceled += OnJumpInput;
    }

    private void FixedUpdate()
    {
        SetMovementCharacter();
        SetRotationCharacter();
    }

    private void SetRotationCharacter()
    {
        _positionToLookAt = new Vector3(_movementCharacter.x, _movementCharacter.y, _movementCharacter.z);
        Quaternion currentRotation = transform.rotation;

        if (IsMoving)
        {
            Quaternion lookAtRotation = Quaternion.LookRotation(_positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, lookAtRotation, _rotationVelocity * Time.deltaTime);
        }
    }

    private void SetMovementCharacter()
    {
        IsGrounded = Physics.CheckSphere(_footPosition.position, 0.5f, _groundLayer);

        _characterController.Move(_movementCharacter * _velocity * Time.deltaTime);

        _verticalVelocity += _gravity * Time.deltaTime;

        _characterController.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _movementCharacterInput = context.ReadValue<Vector2>();
        _movementCharacter = new Vector3(_movementCharacterInput.x, 0, _movementCharacterInput.y);

        IsMoving = _movementCharacterInput.x != 0 || _movementCharacterInput.y != 0;
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        JumpPressed = context.ReadValueAsButton();

        if (JumpPressed && IsGrounded)
        {
            _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        if (IsGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -1f;
        }
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
