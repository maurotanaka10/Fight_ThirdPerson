using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public bool CanDoComboHit = false;
    public bool BackToTheFistCombo = true;
    [SerializeField] private float timeToMove;
    [SerializeField] private float timeToDoCombo;

    private void Awake()
    {
        BackToTheFistCombo = true;
        CanDoComboHit = false;
    }

    public void AttackPlayer(InputAction.CallbackContext context)
    {
        PlayerManager.Instance.SetPlayerAttacking(context.ReadValueAsButton());
        SetAttack();
    }

    private void SetAttack()
    {
        if (PlayerManager.Instance.GetPlayerAttacking())
        {
            StartCoroutine(PlayerCantMove());
            StartCoroutine(TimerToCombo());
        }
    }


    public IEnumerator PlayerCantMove()
    {
        PlayerManager.Instance.SetVelocity(0);
        Debug.Log($"Player nao pode se mexer");
        yield return new WaitForSeconds(timeToMove);
        PlayerManager.Instance.SetVelocity(5);
        Debug.Log($"Player pode se mexer");
    }

    public IEnumerator TimerToCombo()
    {
        BackToTheFistCombo = false;
        CanDoComboHit = true;
        yield return new WaitForSeconds(timeToDoCombo);
        CanDoComboHit = false;
        BackToTheFistCombo = true;
    }
}
