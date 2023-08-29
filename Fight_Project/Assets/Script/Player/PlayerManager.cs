using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private PlayerAttack _playerAttack;
    private PlayerMovement _playerMovement;
    private InputController _inputController;
    private GravityComponent _gravityComponent;

    [SerializeField] private bool _jumpPressed;
    [SerializeField] private bool _isAttacking;
    [SerializeField] private float _velocity;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private int _life;


    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        #endregion
        _playerAttack = GetComponent<PlayerAttack>();
        _playerMovement = GetComponent<PlayerMovement>();
        _inputController = GetComponent<InputController>();
        _gravityComponent = GetComponent<GravityComponent>();

        InputController.onAttack += AttackPlayer;
        InputController.onJump += JumpPlayer;
    }

    private void AttackPlayer(InputAction.CallbackContext context)
    {
        _playerAttack.AttackPlayer(context);
    }

    private void JumpPlayer(InputAction.CallbackContext context)
    {
        _playerMovement.JumpPlayer(context);
    }

    public bool GetJumpPressed()
    {
        return _jumpPressed;
    }

    public void SetJumpPressed(bool _jumpPressed)
    {
        this._jumpPressed = _jumpPressed;
    }

    public bool GetPlayerAttacking()
    {
        return _isAttacking;
    }

    public void SetPlayerAttacking(bool _isAttacking)
    {
        this._isAttacking = _isAttacking;
    }

    public float GetVelocity()
    {
        return _velocity;
    }

    public void SetVelocity(float _playerVelocity)
    {
        this._velocity = _playerVelocity;
    }

    public float GetJumpHeight()
    {
        return _jumpHeight;
    }

    public bool GetIsGrounded()
    {
        return _playerMovement.IsGrounded;
    }

    public float GetCurrentVelocity()
    {
        return _playerMovement.CurrentVelocity;
    }

    public Vector3 GetMovementCharacter()
    {
        return _inputController.MovementCharacter;
    }

    public void SetMovementCharacter(Vector3 _movementCharacter)
    {
        _inputController.MovementCharacter = _movementCharacter;
    }

    public float GetGravity()
    {
        return _gravityComponent.Gravity;
    }
    
    public int GetAttackComboIndex()
    {
        return _playerAttack.AttackComboIndex;
    }

    private void OnDisable()
    {
        InputController.onAttack -= AttackPlayer;
        InputController.onJump -= JumpPlayer;

    }
}
