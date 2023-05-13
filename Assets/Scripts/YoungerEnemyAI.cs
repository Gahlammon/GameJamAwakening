using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class YoungerEnemyAI : MonoBehaviour
{
    [SerializeField]
    private float maxNoise = 10;
    [SerializeField] 
    private float range = 1;
    private float currentNoise = 0;
    public float Speed = 0.1f;
    private Vector3 nextTarget;
    private GameObject[] players;
    private YoungerEnemyStates state;
    private YoungerEnemyStates nextState;
    private Rigidbody rb;
    private NavMeshAgent agent;
    private Animator animator;
    public enum YoungerEnemyStates 
    {
        Sleep,
        Wake,
        Idle,
        Sound,
        Player,
        Attack,
        Death
    }

    private void Start() 
    {
        animator = GetComponent<Animator>();
        if(maxNoise>0)
        {
            state = YoungerEnemyStates.Sleep;
            nextState = YoungerEnemyStates.Sleep;
            animator.Play("BaseLayer.Sleep");
            agent.enabled = false;
        }
        else
        {
            state = YoungerEnemyStates.Idle;
            nextState = YoungerEnemyStates.Idle;
            animator.Play("BaseLayer.Idle");
        }
        players = GameObject.FindGameObjectsWithTag("Player");
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        agent = GetComponent<NavMeshAgent>();
        agent.angularSpeed = 0;
    }

    private void FixedUpdate() 
    {

        switch (state)
        {
            case YoungerEnemyStates.Sleep:
            
                break;
            case YoungerEnemyStates.Idle:
                if(!CheckSight())
                {
                    nextState = YoungerEnemyStates.Idle;
                }
                break;
            case YoungerEnemyStates.Sound:
                agent.SetDestination(nextTarget);
                if(!CheckSight())
                {
                    if(agent.remainingDistance < 0.1)
                    {
                        nextState = YoungerEnemyStates.Idle;
                    }
                    else
                    {
                        nextState = YoungerEnemyStates.Sound;
                    }
                }
                break;
            case YoungerEnemyStates.Player:
                agent.SetDestination(nextTarget);
                if(!CheckSight())
                {
                    nextState = YoungerEnemyStates.Sound;
                }
                RaycastHit hit;
                if(Physics.Raycast(transform.position, nextTarget - transform.position, out hit, range))
                {
                    if(hit.collider.gameObject.CompareTag("Player"))
                    {
                        nextState = YoungerEnemyStates.Attack;
                    }
                }
                break;
            case YoungerEnemyStates.Attack:
                nextState = YoungerEnemyStates.Idle;
                break;
        }
        state = nextState;
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

    private void UseAnimation()
    {
        switch (state)
        {
            case YoungerEnemyStates.Sleep:
                animator.Play("BaseLayer.Sleep");
                break;
            case YoungerEnemyStates.Wake:
                animator.Play("BaseLayer.Wake");
                agent.enabled = true;
                break;
            case YoungerEnemyStates.Idle:
                animator.Play("BaseLayer.Idle");
                break;
            case YoungerEnemyStates.Sound:
                animator.Play("BaseLayer.Run");
                break;
            case YoungerEnemyStates.Player:
                animator.Play("BaseLayer.Run");
                break;
            case YoungerEnemyStates.Attack:
                animator.Play("BaseLayer.Attack");
                break;
            case YoungerEnemyStates.Death:
                animator.Play("BaseLayer.Death");
                break;
        }
    }

    public void Sound(Vector3 source, float amount)
    {
        if(state == YoungerEnemyStates.Sleep)
        {
            currentNoise += amount;
            if(currentNoise <= maxNoise)
            {
                nextState = YoungerEnemyStates.Wake;
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
