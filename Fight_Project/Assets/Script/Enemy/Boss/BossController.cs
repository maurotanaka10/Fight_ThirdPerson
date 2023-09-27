using System;
using System.Collections;
using System.IO.Abstractions.TestingHelpers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class BossController : MonoBehaviour
{
    private EBossStates _currentState;
    public NavMeshAgent _navMeshAgent;
    public SphereCollider _sphereCollider;
    [SerializeField] private PlayerManager _playerManager;
    
    public event Action<EBossStates> OnIdleBoss;
    public event Action<EBossStates> OnPatrolBoss;
    public event Action<EBossStates> OnChaseBoss;
    public event Action<EBossStates> OnAttackNormalBoss;
    public event Action<EBossStates> OnAttack360Boss;
    public event Action<EBossStates> OnHitBoss;
    public event Action<EBossStates> OnDieBoss;
    
    [SerializeField] private float _timeInIdle;
    [SerializeField] private float _visionRange;
    [SerializeField] private float _attackRange;
    public float _velocity;
    [SerializeField] private Vector2 _minPosition;
    [SerializeField] private Vector2 _maxPosition; 
    
    public int LifeBoss;

    private float _delayTimerInIdle;
    private bool _isIdleDelaying;
    private float _distanceFromPlayer;
    private Vector3 _playerPosition;
    private bool _isWalking;
    private Vector3 _movePosition;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _currentState = EBossStates.Idle;
        _navMeshAgent.speed = _velocity;
        _sphereCollider.enabled = false;
    }

    private void Update()
    {
        SetBossStates();
        print($"Esta no estado {_currentState}");
        
        _distanceFromPlayer = 0;
        _playerPosition = Vector3.zero;
        if (PlayerManager.PlayerPositionReference != null)
        {
            _playerPosition = PlayerManager.PlayerPositionReference();
            _distanceFromPlayer = Vector3.Distance(_playerPosition, transform.position);
        }
    }

    private void SetBossStates()
    {
        switch (_currentState)
        {
            case EBossStates.Idle:
                IdleStateHandler();
                break;
            case EBossStates.Patrol:
                PatrolStateHandler();
                break;
            case EBossStates.Chase:
                ChaseStateHandler();
                break;
            case EBossStates.AttackNormal:
                AttackNormalStateHandler();
                break;
            case EBossStates.Attack360:
                Attack360StateHandler();
                break;
            case EBossStates.Die:
                DieStateHandler();
                break;
        }
    }

    private void IdleStateHandler()
    {
        OnIdleBoss?.Invoke(_currentState);
        _isIdleDelaying = true;
        _navMeshAgent.isStopped = true;

        if (_isIdleDelaying)
        {
            _delayTimerInIdle += Time.deltaTime;
            if (_delayTimerInIdle >= _timeInIdle)
            {
                _isIdleDelaying = false;
            }
        }
        if(!_isIdleDelaying)
        {
            _delayTimerInIdle = 0f;
            if (_distanceFromPlayer <= _visionRange)
                _currentState = EBossStates.Chase;
            else if (_distanceFromPlayer > _visionRange)
            {
                _currentState = EBossStates.Patrol;
                _isWalking = false;
            }
        }
    }

    private void PatrolStateHandler()
    {
        OnPatrolBoss?.Invoke(_currentState);
        _navMeshAgent.isStopped = false;

        if (!_isWalking)
        {
            _navMeshAgent.SetDestination(SetBossPosition());
            _isWalking = true;
        }

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _currentState = EBossStates.Idle;
            _isIdleDelaying = true;
        }
        else if (_distanceFromPlayer <= _visionRange)
            _currentState = EBossStates.Chase;
    }

    private void ChaseStateHandler()
    {
        OnChaseBoss?.Invoke(_currentState);
        _navMeshAgent.SetDestination(_playerPosition);
        _navMeshAgent.isStopped = false;

        if (_distanceFromPlayer > _visionRange)
        {
            _currentState = EBossStates.Idle;
            _navMeshAgent.isStopped = true;
            _isIdleDelaying = true;
        }
        else if (_distanceFromPlayer <= _attackRange)
        {
            _currentState = EBossStates.AttackNormal;
            _navMeshAgent.isStopped = true;
        }
        else if (LifeBoss == 1 && _distanceFromPlayer <= _attackRange)
        {
            _currentState = EBossStates.Attack360;
            _navMeshAgent.isStopped = true;
        }
        else if (LifeBoss == 0)
        {
            _currentState = EBossStates.Die;
        }
    }

    private void AttackNormalStateHandler()
    {
       
        OnAttackNormalBoss?.Invoke(_currentState);

        if (_distanceFromPlayer >= _attackRange)
        {
            _currentState = EBossStates.Chase;
            _navMeshAgent.speed = _velocity;
        }
        else if (LifeBoss == 1 && _distanceFromPlayer <= _attackRange)
        {
            _currentState = EBossStates.Attack360;
        }
        else if (LifeBoss == 0)
        {
            _currentState = EBossStates.Die;
            _navMeshAgent.speed = 0f;
        }
    }

    private void Attack360StateHandler()
    {
        OnAttack360Boss?.Invoke(_currentState);
        _navMeshAgent.isStopped = true;
        
        if (_distanceFromPlayer >= _attackRange)
        {
            _currentState = EBossStates.Chase;
        }
        else if(_distanceFromPlayer <= _attackRange)
        {
            _currentState = EBossStates.AttackNormal;
        }
        else if (LifeBoss == 0)
        {
            _currentState = EBossStates.Die;
        }
    }

    private void DieStateHandler()
    {
        OnDieBoss?.Invoke(_currentState);
        _navMeshAgent.isStopped = true;
        _navMeshAgent.speed = 0;
    }
    
    private Vector3 SetBossPosition()
    {
        _movePosition = new Vector3(Random.Range(_minPosition.x, _maxPosition.x), transform.position.y,
            Random.Range(_minPosition.y, _maxPosition.y));

        return _movePosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !_playerManager.IsInvulnerable)
        {
            _playerManager.PlayerLives--;
            print($"acertou o jogador");
        }
    }
}
