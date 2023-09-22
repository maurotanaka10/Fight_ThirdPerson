using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[RequireComponent(typeof(NavMeshAgent))]
public class StandardEnemyController : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private EStandardStates _currentState;

    [SerializeField] private float _velocity;
    [SerializeField] private float _attackRange;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _velocity;
    }
}