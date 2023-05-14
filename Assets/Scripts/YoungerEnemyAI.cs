using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
public class YoungerEnemyAI : NetworkBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float attackRange = 1;

    [Header("References")]
    [SerializeField]
    private NoiseListener noiseListener;

    [SerializeField]
    [ReadOnly]
    private YoungerEnemyStates state;

    [SerializeField]
    [ReadOnly]
    private YoungerEnemyStates nextState;

    private bool dead = false;
    private Vector3 nextTarget;
    private List<ulong> players = new List<ulong>();
    private NavMeshAgent agent;
    private Animator animator;
    private Collider newCollider;
    private GameObject closestPlayer;

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

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.angularSpeed = 0;
        animator = GetComponent<Animator>();
        newCollider = GetComponent<Collider>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            GetPlayers();
            NetworkManager.OnClientConnectedCallback += (id) => players.Add(id);
            NetworkManager.OnClientDisconnectCallback += (id) => players.Remove(id);

            noiseListener.NoiseHeardEvent += (_, noise) => OnNoiseHeardServerRpc(noise.position, noise.value);

            if (noiseListener.Awaken)
            {
                state = YoungerEnemyStates.Idle;
                nextState = YoungerEnemyStates.Idle;
                animator.Play("BaseLayer.Idle");
            }
            else
            {
                state = YoungerEnemyStates.Sleep;
                nextState = YoungerEnemyStates.Sleep;
                animator.Play("BaseLayer.Sleep");
                agent.enabled = false;
            }
        }
    }

    [ServerRpc]
    private void OnNoiseHeardServerRpc(Vector3 source, float noiseLevel)
    {
        if (state == YoungerEnemyStates.Sleep)
        {
            state = YoungerEnemyStates.Wake;
            nextTarget = source;
            UseAnimation();
        }
        if (state != YoungerEnemyStates.Player && state != YoungerEnemyStates.Death)
        {
            nextState = YoungerEnemyStates.Sound;
            nextTarget = source;
        }
    }

    private void GetPlayers()
    {
        players = new List<ulong>();

        foreach (var client in NetworkManager.Singleton.ConnectedClientsIds)
        {
            print(client);
            players.Add(client);
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            switch (state)
            {
                case YoungerEnemyStates.Sleep:
                    break;
                case YoungerEnemyStates.Idle:
                    nextState = YoungerEnemyStates.Idle;
                    CheckSightServerRpc();
                    break;
                case YoungerEnemyStates.Sound:
                    agent.SetDestination(nextTarget);
                    if (agent.remainingDistance < 0.1)
                    {
                        nextState = YoungerEnemyStates.Idle;
                    }
                    else
                    {
                        nextState = YoungerEnemyStates.Sound;
                    }
                    CheckSightServerRpc();
                    break;
                case YoungerEnemyStates.Player:
                    agent.SetDestination(nextTarget);
                    nextState = YoungerEnemyStates.Sound;
                    CheckSightServerRpc();
                    AttackPlayerServerRpc();
                    break;
                case YoungerEnemyStates.Attack:
                    nextState = YoungerEnemyStates.Idle;
                    break;
                case YoungerEnemyStates.Wake:
                    nextState = YoungerEnemyStates.Sound;
                    break;
                case YoungerEnemyStates.Death:
                    nextState = YoungerEnemyStates.Death;
                    break;
            }
            state = nextState;
        }
    }

    [ServerRpc]
    private void CheckSightServerRpc()
    {
        float minDistance = Mathf.Infinity;
        foreach (ulong playerId in players)
        {
            GameObject player = NetworkManager.Singleton.ConnectedClients[playerId].PlayerObject.gameObject;
            if (Physics.Raycast(transform.position, player.transform.position - transform.position, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Default", "Player")))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    float sqrDistance = (hit.transform.position - transform.position).sqrMagnitude;
                    if (sqrDistance < minDistance)
                    {
                        minDistance = sqrDistance;
                        nextState = YoungerEnemyStates.Player;
                        nextTarget = hit.transform.position;
                        closestPlayer = hit.collider.gameObject;
                    }
                }
            }
        }
    }

    [ServerRpc]
    private void AttackPlayerServerRpc()
    {
        if (Physics.Raycast(transform.position, closestPlayer.transform.position - transform.position, out RaycastHit hit, attackRange, LayerMask.GetMask("Default", "Player")))
        {
            if(hit.collider.tag == "Player")
            {
                state = YoungerEnemyStates.Attack;
                UseAnimation();
                closestPlayer.GetComponent<PlayerDeathHandler>().KillPlayerClientRpc();
            }
            
        }
    }

    //Called by animation events
    private void UseAnimation()
    {
        if(dead)
        {
            animator.enabled = false;
        }
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
                dead = true;
                break;
        }
    }

    [ServerRpc]
    public void GetKilledServerRPC()
    {
        if (IsOwner)
        {
            newCollider.enabled = false;
            state = YoungerEnemyStates.Death;
            nextState = YoungerEnemyStates.Death;
            UseAnimation();
        }
    }
}
