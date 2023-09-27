using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class StandardEnemyController : MonoBehaviour
{
    #region Components

    private NavMeshAgent _navMeshAgent;
    private EStandardStates _currentState;
    private CapsuleCollider _capsuleCollider;
    [SerializeField] private PlayerManager _playerManager;

    #endregion

    #region Serialize Variables

    [SerializeField] private float _lifes;
    [SerializeField] private float _velocityWalk;
    [SerializeField] private float _velocityRun;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _visionRange;
    [SerializeField] private float _idleTime;
    [SerializeField] private Vector2 _minPosition;
    [SerializeField] private Vector2 _maxPosition;

    #endregion

    #region Private Variables

    private float _distanceFromPlayer;
    private Vector3 _playerPosition;
    private Vector3 _movePosition;
    private float _delayTimerIdle = 0f;
    private bool _isIdleDelaying;
    private bool _isWalking;
    public bool EnemyStandardIsDead;
    private bool _hitPlayer;
    #endregion

    #region Actions

    public event Action<EStandardStates> OnIdle;
    public event Action<EStandardStates> OnPatrol;
    public event Action<EStandardStates> OnChase;
    public event Action<EStandardStates> OnDie;

    #endregion

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _navMeshAgent.speed = _velocityWalk;

        _currentState = EStandardStates.Idle;
        _isIdleDelaying = true;
        EnemyStandardIsDead = false;
    }

    private void Update()
    {
        SetEnemyCurrentState();

        _distanceFromPlayer = 0;
        _playerPosition = Vector3.zero;
        if (PlayerManager.PlayerPositionReference != null)
        {
            _playerPosition = PlayerManager.PlayerPositionReference();
            _distanceFromPlayer = Vector3.Distance(_playerPosition, transform.position);
        }
    }

    private void SetEnemyCurrentState()
    {
        switch (_currentState)
        {
            case EStandardStates.Idle:
                IdleStateHandler();
                break;
            case EStandardStates.Patrol:
                PatrolStateHandler();
                break;
            case EStandardStates.Chase:
                ChaseStateHandler();
                break;
        }

        if (EnemyStandardIsDead)
        {
            Destroy(this.gameObject);
        }
    }

    private void IdleStateHandler()
    {
        OnIdle?.Invoke(_currentState);
        _navMeshAgent.isStopped = true;

        if (_isIdleDelaying)
        {
            _delayTimerIdle += Time.deltaTime;
            if (_delayTimerIdle >= _idleTime)
            {
                _isIdleDelaying = false;
            }
        }

        if (!_isIdleDelaying)
        {
            _delayTimerIdle = 0f;

            if (_distanceFromPlayer <= _visionRange)
                _currentState = EStandardStates.Chase;
            else if (_distanceFromPlayer > _visionRange)
            {
                _currentState = EStandardStates.Patrol;
                _isWalking = false;
            }
        }
    }

    private void PatrolStateHandler()
    {
        OnPatrol?.Invoke(_currentState);
        _navMeshAgent.speed = _velocityWalk;
        _navMeshAgent.isStopped = false;

        if (!_isWalking)
        {
            _navMeshAgent.SetDestination(SetEnemyPosition());
            _isWalking = true;
        }


        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _currentState = EStandardStates.Idle;
            _navMeshAgent.isStopped = true;
            _isIdleDelaying = true;
        }

        if (_distanceFromPlayer <= _visionRange)
        {
            _currentState = EStandardStates.Chase;
        }
    }

    private void ChaseStateHandler()
    {
        OnChase?.Invoke(_currentState);
        _navMeshAgent.speed = _velocityRun;
        _navMeshAgent.SetDestination(_playerPosition);
        _navMeshAgent.isStopped = false;

        if (_distanceFromPlayer > _visionRange)
        {
            _currentState = EStandardStates.Idle;
            _navMeshAgent.isStopped = true;
            _isIdleDelaying = true;
        }
    }

    private Vector3 SetEnemyPosition()
    {
        _movePosition = new Vector3(Random.Range(_minPosition.x, _maxPosition.x), transform.position.y,
            Random.Range(_minPosition.y, _maxPosition.y));

        return _movePosition;
    }
}