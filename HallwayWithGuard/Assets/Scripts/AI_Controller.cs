using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Controller : MonoBehaviour
{
    private GameObject _playerRef;
    private bool _hasPlayer = false;

    [SerializeField]
    bool _Wait;
    [SerializeField]
    float _waitTime = 3f;
    [SerializeField]
    float _directionSwitchProbability = 0.2f;
    [SerializeField]
    List<WayPoint> _patrolPoints;

    Animator animControl;
    NavMeshAgent _navMeshAgent;
    int _currentPatrolIndex;
    bool _isTravelling;
    bool _isWaiting;
    bool _patrolForward;
    float _waitTimer;

    
    // Start is called before the first frame update
    void Start()
    {
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        animControl = GetComponent<Animator>();
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if(_navMeshAgent == null)
        {
            print("there is no navMeshAgent component attached to:" + gameObject.name);
        }
        else
        {
            if(_patrolPoints != null && _patrolPoints.Count >= 2){
                _currentPatrolIndex = 0;
                SetDestination();
            }
            else
            {
                print("Not Enough Patrol Points Set");
            }
        }

    }

    public void Update()
    {
        if(_isTravelling && _navMeshAgent.remainingDistance <= 1.0f)
        {
            _isTravelling = false;

            if (_Wait)
            {
                _isWaiting = true;
                _waitTimer = 0;
            }
            else
            {
                ChangePatrolPoint();
                SetDestination();
            }
        }

        if (_isWaiting)
        {
            _waitTimer += Time.deltaTime;
            if(_waitTimer >= _waitTime)
            {
                _isWaiting = false;

                ChangePatrolPoint();
                SetDestination();
            }
        }
    }

    private void SetDestination()
    {
        if (_hasPlayer)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _playerRef.transform.position);
            if (distanceToPlayer > 1.5f)
            {
                _navMeshAgent.SetDestination(_playerRef.transform.position);
            }
        }

        if (_patrolPoints != null && _hasPlayer == false)
        {
            Vector3 target = _patrolPoints[_currentPatrolIndex].transform.position;
            _navMeshAgent.SetDestination(target);
            _isTravelling = true;
        }
    }

    private void ChangePatrolPoint()
    {
        if(UnityEngine.Random.Range(0f,1f) <= _directionSwitchProbability)
        {
            _patrolForward = !_patrolForward;
        }

        if (_patrolForward)
        {
            _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
        }
        else if (--_currentPatrolIndex < 0)
        {
            _currentPatrolIndex = _patrolPoints.Count - 1;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        Vector3 dir = _playerRef.transform.position - new Vector3(transform.position.x, _playerRef.transform.position.y, transform.position.z);

        if(other.tag == "Player")
        {
            /*RaycastHit hit;
            if(Physics.Raycast(transform.position, dir, out hit, 10))
            {
                Debug.DrawRay(transform.position, dir * 10, Color.yellow);
                print(hit.rigidbody.gameObject.tag);
                if (hit.Equals(_playerRef))
                {
                    _hasPlayer = true;
                    SetDestination();
                }
            }*/
            _hasPlayer = true;
            SetDestination();
        }
    }

    public bool Freeze(bool isStopped)
    {
        _navMeshAgent.isStopped = isStopped;
        animControl.SetBool("IsStoped", isStopped);

        return _navMeshAgent.isStopped;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _hasPlayer = false;
            SetDestination();
        }
    }

}
