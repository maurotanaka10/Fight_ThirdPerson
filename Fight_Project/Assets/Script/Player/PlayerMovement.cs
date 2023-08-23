using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;
    private PlayerInputSystem _playerInputSystem;
    public static PlayerMovement instance;

    private Vector2 _movementCharacterInput;
    private Vector3 _movementCharacter;
    private Vector3 _positionToLookAt;
    private Vector3 _cameraRelativeMovement;
    private float _verticalVelocity;
    private float _gravity = -9.81f;

    public bool IsGrounded;
    public float _currentVelocity;
    public bool JumpPressed;

    public float Velocity;
    [SerializeField] private float _rotationVelocity;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private Transform _footPosition;
    [SerializeField] private LayerMask _groundLayer;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

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
        GravityHandler();

        Debug.Log(IsGrounded);
        IsGrounded = Physics.CheckSphere(_footPosition.position, 0.5f, _groundLayer);
    }

    private void SetRotationCharacter()
    {
        float rotationFactorPerFrame = 10;
        Vector3 positionToLookAt;
        positionToLookAt.x = _cameraRelativeMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = _cameraRelativeMovement.z;
        Quaternion currentRotation = transform.rotation;

        if (_currentVelocity != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    private void SetMovementCharacter()
    {
        _currentVelocity = _characterController.velocity.magnitude/Velocity;

        _cameraRelativeMovement = ConverToCameraSpace(_movementCharacter);
        _characterController.Move(_cameraRelativeMovement * Velocity * Time.deltaTime);
    }

    private void GravityHandler()
    {
        _verticalVelocity += _gravity * Time.deltaTime;
    }

    private Vector3 ConverToCameraSpace(Vector3 vectorToRotate)
    {
        float currentYValue = vectorToRotate.y;

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 cameraForwardZproduct = vectorToRotate.z * cameraForward;
        Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

        Vector3 vectorRotatedToCameraSpace = cameraForwardZproduct + cameraRightXProduct;
        vectorRotatedToCameraSpace.y = currentYValue;
        return vectorRotatedToCameraSpace;
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _movementCharacterInput = context.ReadValue<Vector2>();
        _movementCharacter = new Vector3(_movementCharacterInput.x, _verticalVelocity, _movementCharacterInput.y);
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
