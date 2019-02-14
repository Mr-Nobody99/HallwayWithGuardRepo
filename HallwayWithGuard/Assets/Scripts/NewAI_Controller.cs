using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewAI_Controller : MonoBehaviour
{

    private GameObject playerRef;
    private float dot;
    int currentPatrolIndex;
    private bool isPatrolling = true;
    private bool persuePlayer = false;
    private bool isStopped = false;

    Animator animController;
    NavMeshAgent navMeshAgent;
    RaycastHit Hit;

    [SerializeField]
    float directionalSwitchProbability = 0.2f;
    [SerializeField]
    List<WayPoint> patrolPoints;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        animController = this.GetComponent<Animator>();
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        if(navMeshAgent == null)
        {
            print("There is no navMeshAgent component attached to: " + gameObject.name);
        }
        else
        {
           if(patrolPoints != null && patrolPoints.Count >= 2)
            {
                currentPatrolIndex = 0;
                navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].transform.position);
                isPatrolling = true;
            }
           else
            {
                print("Not Enough Patrol Points Set");
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
       if(!isStopped) Gizmos.DrawSphere(Hit.point, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isStopped && Physics.SphereCast(transform.position + new Vector3(0f,3f,0f), 5.0f, transform.forward, out Hit, 50f))
        {
            Debug.DrawLine(transform.position + new Vector3(0f, 3f, 0f), Hit.point, Color.green);
            if(Hit.collider.tag == "Player")
            {
                isPatrolling = false;
                animController.SetBool("persuePlayer", true);
                navMeshAgent.SetDestination(playerRef.transform.position);
                navMeshAgent.speed = 6.5f;
            }
            else
            {
                isPatrolling = true;
                animController.SetBool("persuePlayer", false);
                navMeshAgent.speed = 4.25f;
            }
        }

        dot = Vector3.Dot(playerRef.transform.forward, transform.position - playerRef.transform.position);

        if (dot < 0) SetIsStopped(true);
        else if (dot > 0) SetIsStopped(false);

        if(isPatrolling && navMeshAgent.remainingDistance <= 1.0f)
        {
            ChangePatrolPoint();
        }

    }

    private void ChangePatrolPoint()
    {
        if(UnityEngine.Random.Range(0f,1f) <= directionalSwitchProbability)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }
        else if (--currentPatrolIndex < 0)
        {
            currentPatrolIndex = patrolPoints.Count - 1;
        }

        navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].transform.position);

    }

    private void SetIsStopped(bool shouldStop)
    {
        isStopped = shouldStop;
        navMeshAgent.isStopped = shouldStop;
        animController.SetBool("isStopped", shouldStop);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            print("should die.");
            SceneManager.LoadScene(2);
        }
    }
}
