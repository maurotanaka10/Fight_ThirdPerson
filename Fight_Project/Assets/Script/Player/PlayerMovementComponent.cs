using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementComponent : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;
    
    private Vector3 _currentMovement;
    private Vector3 _cameraRelativeMovement;
    private Vector2 _inputData;

    private float _playerVelocity;
    private const float _gravityValue = -9.81f;
    private float _gravityVelocity;

    private bool _isMoving;

    public CharacterController characterController;

    [SerializeField] private float _jumpPower = 1;
    [SerializeField] private float _gravityMultiplier = 3f;

    private void Awake()
    {
        PlayerManager.HandleMoveInput += SetMoveInfo;
        PlayerManager.HandleJumpInput += MakePlayerJump;
        PlayerManager._characterControllerReference = GetCharacterController;
        PlayerManager.PlayerPositionReference = GetPlayerPosition;

        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MoveHandler(_inputData);
        GravityHandler();
    }

    private void MakePlayerJump(bool inputValue, int numberOfJumps)
    {
        if (inputValue == false) return;
        if (characterController.isGrounded == false && numberOfJumps > 2) return;

        _gravityVelocity = _jumpPower;

    }

    private void SetMoveInfo(InputAction.CallbackContext context, float velocity)
    {
        _playerVelocity = velocity;
        _inputData = context.ReadValue<Vector2>();
        _isMoving = _inputData.x != 0 || _inputData.y != 0;
    }

    private void MoveHandler(Vector2 inputData)
    {
        _currentMovement.x = inputData.x;
        _currentMovement.z = inputData.y;

        _cameraRelativeMovement = ConvertToCameraSpace(_currentMovement);
        characterController.Move(_cameraRelativeMovement * _playerVelocity * Time.deltaTime);
        RotationHandler();
    }

    private void RotationHandler()
    {
        float _rotationFactorPerFrame = 10;
        Vector3 _positionToLookAt;
        _positionToLookAt.x = _cameraRelativeMovement.x;
        _positionToLookAt.y = 0f;
        _positionToLookAt.z = _cameraRelativeMovement.z;
        Quaternion _currentRotation = transform.rotation;

        if (_isMoving)
        {
            Quaternion _targetRotation = Quaternion.LookRotation(_positionToLookAt);
            transform.rotation = Quaternion.Slerp(_currentRotation, _targetRotation, _rotationFactorPerFrame * Time.deltaTime);
        }
    }

    private Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
        float _currentYValue = vectorToRotate.y;

        Vector3 _cameraForward = Camera.main.transform.forward;
        Vector3 _cameraRight = Camera.main.transform.right;

        _cameraForward.y = 0;
        _cameraRight.y = 0;

        Vector3 _cameraForwardZProduct = _cameraForward * vectorToRotate.z;
        Vector3 _cameraRightXProduct = _cameraRight * vectorToRotate.x;

        Vector3 _directionToMovePlayer = _cameraForwardZProduct + _cameraRightXProduct;
        _directionToMovePlayer.y = _currentYValue;

        return _directionToMovePlayer;
    }
    
    private void GravityHandler()
    {
        if (characterController.isGrounded && _gravityVelocity < 0f)
        {
            _gravityVelocity = -1f;
        }
        else
        {
            _gravityVelocity += _gravityValue * _gravityMultiplier * Time.deltaTime;
        }
        
        _currentMovement.y = _gravityVelocity;
    }
    private CharacterController GetCharacterController()
    {
        return characterController;
    }

    private Vector3 GetPlayerPosition()
    {
        return transform.position;
    }

    private void OnDisable()
    {
        PlayerManager.HandleMoveInput -= SetMoveInfo;
        PlayerManager.HandleJumpInput -= MakePlayerJump;
    }
}