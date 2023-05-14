using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemSpawner : NetworkBehaviour
{
    [SerializeField]
    private Transform[] positions;

    [SerializeField]
    private GameObject[] prefabs;

    [SerializeField]
    private float spawnPropabilityPercent;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer)
        {
            enabled = false;
            return;
        }

        foreach (Transform targetTransform in positions)
        {
            if (Random.Range(0, 99) < spawnPropabilityPercent)
            {
                GameObject newPickup = Instantiate(prefabs[Random.Range(0, prefabs.Length)], targetTransform.position, Quaternion.identity);
                newPickup.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}
