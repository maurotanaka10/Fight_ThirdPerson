using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityComponent : MonoBehaviour
{
    public float Gravity = -9.81f;

    private void Awake()
    {

    }

    private void FixedUpdate()
    {
        GravityHandler();
    }

    private void GravityHandler()
    {
        Vector3 _newMovementCharacter = PlayerManager.Instance.GetMovementCharacter();
        _newMovementCharacter.y += Gravity * Time.deltaTime;
        PlayerManager.Instance.SetMovementCharacter(_newMovementCharacter);
    }
}
