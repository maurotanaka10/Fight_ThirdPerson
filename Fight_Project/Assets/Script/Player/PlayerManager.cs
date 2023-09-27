using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerAttack _playerAttack;
    public static event Action<InputAction.CallbackContext, float> HandleMoveInput;
    public static event Action<bool, int> HandleJumpInput;
    public static event Action<bool> HandleAttackInput;
    public event Action<int> HandleKillsEnemy;
    public static event Action<bool> OnDead;

    public delegate CharacterController CharacterControllerReference();

    public static CharacterControllerReference _characterControllerReference;

    public delegate Vector3 PlayerPosition();

    public static PlayerPosition PlayerPositionReference;

    private int _numberOfJumps = 0;
    private bool isDead;
    
    [SerializeField] private float _velocity = 10;
    public int PlayerLives = 3;
    public bool IsInvulnerable;

    private void Awake()
    {
        PlayerManagerSetUpListeners();
    }

    private void Update()
    {
        if (PlayerLives <= 0)
        {
            isDead = true;
            OnDead?.Invoke(isDead);
        }
    }

    private void PlayerManagerSetUpListeners()
    {
        GameManager.OnMoveInputContextReceived += HandleMove;
        GameManager.OnJumpInputContextReceived += HandleJump;
        GameManager.OnAttackInputContextReceived += HandleAttack;
        _playerAttack.OnKillsEnemy += HandleHitEnemyInAttack;
    }

    private void HandleHitEnemyInAttack(int numberOfKills)
    {
        HandleKillsEnemy?.Invoke(numberOfKills);
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

        IsInvulnerable = true;
        StartCoroutine(TurnOffInvulnerable());
    }

    private void OnDisable()
    {
        GameManager.OnMoveInputContextReceived -= HandleMove;
        GameManager.OnJumpInputContextReceived -= HandleJump;
        GameManager.OnAttackInputContextReceived -= HandleAttack;
    }

    private IEnumerator TurnOffInvulnerable()
    {
        yield return new WaitForSeconds(1f);
        IsInvulnerable = false;
    }
}