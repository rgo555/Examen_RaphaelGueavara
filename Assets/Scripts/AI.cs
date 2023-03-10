using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AI : MonoBehaviour
{
    enum State
    {
        Patrolling,
        Chasing,
        Attacking,
    }

    State currentState;
    NavMeshAgent agent;

    public Transform[] destinationPoints;
    int destinationIndex= 0;

    public Transform player;
    [SerializeField]
    float visionRange;
    float attackRange;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }



    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Patrolling; 

        destinationIndex = Random.Range(0, destinationPoints.Length);
    }
    

    // Update is called once per frame
    void Update() 
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
            break;

            case State.Chasing:
                Chase();
            break;

            case State.Attacking:
                Attack();
            break;
        }
    }

    void Patrol()
    {
        agent.destination = destinationPoints[destinationIndex].position;

        if(Vector3.Distance(transform.position, destinationPoints[destinationIndex].position) < 1f)
        {
            destinationIndex = Random.Range(0, destinationPoints.Length);
        }

        if(DistanceToTarget(visionRange))
        {
            currentState = State.Chasing;
        }
    }

    bool DistanceToTarget(float distance)
    {
        if(Vector3.Distance(transform.position, destinationPoints[destinationIndex].position) < distance)
        {
            return true;
        }
        return false;
    }
    
    void Chase() 
    {
        agent.destination = player.position; 

        if(DistanceToTarget(visionRange) == false)
        {
            currentState = State.Patrolling; 
        }

        if(DistanceToTarget(attackRange))
        {
            currentState = State.Attacking;
        }
    }

    void Attack () 
    {
        Debug.Log("Ataque");

        if (!DistanceToTarget(attackRange))
        {
            currentState = State.Chasing;
        }
    }

    void OnDrawGizmos()
    {
        foreach (Transform point in destinationPoints)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(point.position, 1);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }

}
