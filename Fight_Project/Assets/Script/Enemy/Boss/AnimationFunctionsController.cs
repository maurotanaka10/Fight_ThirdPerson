using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationFunctionsController : MonoBehaviour
{
    [SerializeField] private BossController _bossController;

    private void StopEnemy()
    {
        _bossController._navMeshAgent.isStopped = true;
        _bossController._navMeshAgent.speed = 0f;
    }
    private void ReturnEnemy()
    {
        _bossController._navMeshAgent.isStopped = false;
        _bossController._navMeshAgent.speed = _bossController._velocity;
    }

    private void AttackColliderActive()
    {
        _bossController._sphereCollider.enabled = true;
    }
    private void AttackColliderDesactive()
    {
        _bossController._sphereCollider.enabled = false;
    }
}
