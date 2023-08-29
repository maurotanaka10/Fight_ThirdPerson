using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public int AttackComboIndex = 0;
    private bool isInCombat;
    [SerializeField] private float timeToMove;
    [SerializeField] private float timeToResetCombo;

    private void Update()
    {
        if(PlayerManager.Instance.GetPlayerAttacking() && !isInCombat)
        {
            SetAttack();
        }
    }

    public void AttackPlayer(InputAction.CallbackContext context)
    {
        PlayerManager.Instance.SetPlayerAttacking(context.ReadValueAsButton());
    }

    private void SetAttack()
    {
        isInCombat = true;
        AttackComboIndex = 0;
        AttackComboIndex = AttackComboIndex + 1;
        StartCoroutine(ResetCombo());
    }


    public IEnumerator PlayerCantMove()
    {
        PlayerManager.Instance.SetVelocity(0);
        Debug.Log($"Player nao pode se mexer");
        yield return new WaitForSeconds(timeToMove);
        PlayerManager.Instance.SetVelocity(5);
        Debug.Log($"Player pode se mexer");
    }

    public IEnumerator ResetCombo()
    {
        yield return new WaitForSeconds(timeToResetCombo);
        isInCombat = false;
        AttackComboIndex = 0;
    }
}
