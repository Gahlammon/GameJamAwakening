using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class YoungerEnemyAI : MonoBehaviour
{
    [SerializeField]
    private float maxNoise = 10;
    private float currentNoise = 0;
    public float Speed = 0.1f;
    private Vector3 nextTarget;
    private GameObject[] players;
    private YoungerEnemyStates state;
    private YoungerEnemyStates nextState;
    private Rigidbody rb;
    private NavMeshAgent agent;
    public enum YoungerEnemyStates 
    {
        Sleep,
        Patrol,
        Sound,
        Player
    }

    private void Start() 
    {
        if(maxNoise>0)
        {
            state = YoungerEnemyStates.Sleep;
            nextState = YoungerEnemyStates.Sleep;
        }
        else
        {
            state = YoungerEnemyStates.Patrol;
            nextState = YoungerEnemyStates.Patrol;
        }
        players = GameObject.FindGameObjectsWithTag("Player");
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(transform.position + transform.forward);
        nextTarget = transform.position + transform.forward;
    }

    private void FixedUpdate() 
    {
        if(state == YoungerEnemyStates.Sleep)
        {
            CheckSight();
            if(nextState == YoungerEnemyStates.Patrol)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, nextTarget - transform.position, out hit, 1))
                {
                    if(!hit.collider.gameObject.CompareTag("Player"))
                    {
                        nextTarget = Random.insideUnitSphere;
                        Debug.Log(nextTarget);
                        nextTarget.y = 0;
                        nextTarget = nextTarget.normalized;
                    }
                }
            }
        }
        state = nextState;
        agent.SetDestination(nextTarget);
    }

    private bool CheckSight()
    {
        foreach(GameObject player in players)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, player.transform.position - transform.position, out hit))
            {
                if(hit.collider.gameObject.CompareTag("Player"))
                {
                    nextState = YoungerEnemyStates.Player;
                    nextTarget = hit.transform.position;
                    return true;
                }
            }
        }
        return false;
    }

    public void Sound(Vector3 source, float amount)
    {
        if(state == YoungerEnemyStates.Sleep)
        {
            currentNoise += amount;
            if(currentNoise <= maxNoise)
            {
                nextState = YoungerEnemyStates.Sound;
                nextTarget = source;
            }
        }
        if(state != YoungerEnemyStates.Player)
        {
            nextState = YoungerEnemyStates.Sound;
            nextTarget = source;
        }
    }

    
}
