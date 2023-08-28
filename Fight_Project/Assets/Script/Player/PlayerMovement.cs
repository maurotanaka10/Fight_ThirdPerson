using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;
    private CameraComponent _cameraComponent;

    private Vector3 _positionToLookAt;
    private Vector3 _cameraRelativeMovement;
    public float CurrentVelocity;
    public bool IsGrounded;

    [SerializeField] private float _rotationVelocity;
    [SerializeField] private Transform _footPosition;
    [SerializeField] private LayerMask _groundLayer;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _cameraComponent = new CameraComponent();
    }

    private void FixedUpdate()
    {
        SetMovementCharacter();
        SetRotationCharacter();

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

        if (CurrentVelocity != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    private void SetMovementCharacter()
    {
        CurrentVelocity = _characterController.velocity.magnitude/PlayerManager.Instance.GetVelocity();

        _cameraRelativeMovement = _cameraComponent.ConverToCameraSpace(PlayerManager.Instance.GetMovementCharacter());
        _characterController.Move(_cameraRelativeMovement * PlayerManager.Instance.GetVelocity() * Time.deltaTime);
    }

    public void JumpPlayer(InputAction.CallbackContext context)
    {
        PlayerManager.Instance.SetJumpPressed(context.ReadValueAsButton());
        Vector3 _newMovementCharacter = PlayerManager.Instance.GetMovementCharacter();

        if (PlayerManager.Instance.GetJumpPressed() && PlayerManager.Instance.GetIsGrounded())
        {
            _newMovementCharacter.y = Mathf.Sqrt(PlayerManager.Instance.GetJumpHeight() * -2f * PlayerManager.Instance.GetGravity());
            PlayerManager.Instance.SetMovementCharacter(_newMovementCharacter);
        }

        if (PlayerManager.Instance.GetIsGrounded() && PlayerManager.Instance.GetMovementCharacter().y < 0)
        {
            _newMovementCharacter.y = -1f;
            PlayerManager.Instance.SetMovementCharacter(_newMovementCharacter);
        }
    }
}
