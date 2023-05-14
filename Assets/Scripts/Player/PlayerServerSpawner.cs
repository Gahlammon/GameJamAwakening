using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Player
{
    [DefaultExecutionOrder(0)]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerNetworkComponentController))]
    public class PlayerServerSpawner : NetworkBehaviour
    {
        private CharacterController controller;
        private PlayerNetworkComponentController playerNetworkComponentController;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            playerNetworkComponentController = GetComponent<PlayerNetworkComponentController>();
        }

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                enabled = false;
                return;
            }

            OnServerSpawnPlayer();

            base.OnNetworkSpawn();
        }

        void OnServerSpawnPlayer()
        {
            Transform spawnPoint = ServerPlayerSpawnPoints.Instance.ConsumeNextSpawnPoint();
            Vector3 spawnPosition = spawnPoint ? spawnPoint.position : Vector3.zero;
            controller.transform.position = spawnPosition;

            playerNetworkComponentController.Id = NetworkManager.ConnectedClients.Count;
        }
    }
}