using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[DefaultExecutionOrder(0)]
public class PlayerServerSpawner : NetworkBehaviour
{
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
        transform.position = spawnPosition;
    }
}
