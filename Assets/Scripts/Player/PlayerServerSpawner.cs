using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Player
{
    [DefaultExecutionOrder(0)]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerServerSpawner : NetworkBehaviour
    {
        private CharacterController controller;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
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
            controller.Move(spawnPosition - transform.position);
        }
    }
}