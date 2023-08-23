using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private PlayerInputSystem _playerInputSystem;
    public static PlayerAttack instance;

    public bool IsAttacking;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        _playerInputSystem = new PlayerInputSystem();

        _playerInputSystem.Player.Attack.started += OnAttacking;
        _playerInputSystem.Player.Attack.canceled += OnAttacking;
    }


    private void OnAttacking(InputAction.CallbackContext context)
    {
        IsAttacking = context.ReadValueAsButton();

        if (IsAttacking)
        {
            StartCoroutine(PlayerCantMove());
        }
    }

    IEnumerator PlayerCantMove()
    {
        PlayerMovement.instance.Velocity = 0;
        Debug.Log($"Player nao pode se mexer");
        yield return new WaitForSeconds(0.5f);
        PlayerMovement.instance.Velocity = 5;
        Debug.Log($"Player pode se mexer");
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
