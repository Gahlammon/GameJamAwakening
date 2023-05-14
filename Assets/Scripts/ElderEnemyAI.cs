using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ElderEnemyAI : NetworkBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float attackRange = 1;
    [SerializeField]
    private float speed = 5;

    [Header("References")]
    [SerializeField]
    private NoiseListener noiseListener;

    [SerializeField]
    [ReadOnly]
    private ElderEnemyStates state;

    [SerializeField]
    [ReadOnly]
    private ElderEnemyStates nextState;

    private Vector3 nextTarget;
    private List<ulong> players = new List<ulong>();
    private Animator animator;
    private GameObject closestPlayer;

    public enum ElderEnemyStates
    {
        Sleep,
        Wake,
        Player,
        Attack,
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            GetPlayers();
            NetworkManager.OnClientConnectedCallback += (id) => players.Add(id);
            NetworkManager.OnClientDisconnectCallback += (id) => players.Remove(id);

            noiseListener.NoiseHeardEvent += (_, noise) => OnNoiseHeardServerRpc(noise.position, noise.value);

            state = ElderEnemyStates.Sleep;
            nextState = ElderEnemyStates.Sleep;
            animator.enabled = false;
        }
    }

    [ServerRpc]
    private void OnNoiseHeardServerRpc(Vector3 source, float noiseLevel)
    {
        if (state == ElderEnemyStates.Sleep)
        {
            state = ElderEnemyStates.Wake;
            animator.enabled = true;
            UseAnimation();
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
            if(state != ElderEnemyStates.Sleep)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextTarget, speed * Time.deltaTime);
            }
            switch (state)
            {
                case ElderEnemyStates.Sleep:
                    break;
                case ElderEnemyStates.Player:
                    CheckSightServerRpc();
                    AttackPlayerServerRpc();
                    break;
                case ElderEnemyStates.Attack:
                    nextState = ElderEnemyStates.Player;
                    break;
                case ElderEnemyStates.Wake:
                    nextState = ElderEnemyStates.Player;
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
            float sqrDistance = (player.transform.position - transform.position).sqrMagnitude;
            if (sqrDistance < minDistance)
            {
                minDistance = sqrDistance;
                nextState = ElderEnemyStates.Player;
                nextTarget = player.transform.position;
                nextTarget.y = transform.position.y;
                closestPlayer = player;
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
                state = ElderEnemyStates.Attack;
                UseAnimation();
                closestPlayer.GetComponent<PlayerDeathHandler>().KillPlayerClientRpc();
            }
            
        }
    }

    //Called by animation events
    private void UseAnimation()
    {
        if(state == ElderEnemyStates.Wake)
        {
            animator.Play("BaseLayer.Wake");
        }
        else
        {
            animator.Play("BaseLayer.Idle");
        }
    }
}
