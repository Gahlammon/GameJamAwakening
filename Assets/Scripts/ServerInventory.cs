using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ServerInventory : NetworkBehaviour
{
    public NetworkVariable<int> Pipes => pipes;
    public NetworkVariable<int> Bricks => bricks;
    public NetworkVariable<int> Bootles => bootles;
    public NetworkVariable<int> Knifes => knifes;
    public NetworkVariable<int> Parts => parts;
    public NetworkVariable<int> Medalions => medalions;

    private NetworkVariable<int> pipes = new NetworkVariable<int>(0);
    private NetworkVariable<int> bricks = new NetworkVariable<int>(0);
    private NetworkVariable<int> bootles = new NetworkVariable<int>(0);
    private NetworkVariable<int> knifes = new NetworkVariable<int>(0);
    private NetworkVariable<int> parts = new NetworkVariable<int>(0);
    private NetworkVariable<int> medalions = new NetworkVariable<int>(0);

    private const int maxMedalionParts = 3;

    private Dictionary<PickupController.PickupType, (GameObject prefab, NetworkVariable<int> count)> inventory;

    private void Awake()
    {
        inventory = new Dictionary<PickupController.PickupType, (GameObject prefab, NetworkVariable<int> count)>
        {
            { PickupController.PickupType.Bootle, (null, bootles) },
            { PickupController.PickupType.Pipe, (null, pipes) },
            { PickupController.PickupType.Brick, (null, bricks) },
            { PickupController.PickupType.Knife, (null, knifes) },
            { PickupController.PickupType.Part, (null, parts) },
            { PickupController.PickupType.Medalion, (null, medalions) }
        };
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer)
        {
            enabled = false;
            return;
        }
    }

    public void RemoveMedalionsFromOthers()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            client.PlayerObject.GetComponent<ServerInventory>().Parts.Value = 0;
        }
    }

    [ServerRpc]
    public void PickupPickupServerRpc(ulong objectId)
    {
        print("Pickup RPC");
        NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objectId, out NetworkObject objectToPickup);
        if (objectToPickup != null)
        {
            PickupController controller = objectToPickup.GetComponent<PickupController>();
            (GameObject prefab, NetworkVariable<int> count) pair = inventory[controller.Type];

            if (controller.Type == PickupController.PickupType.Part)
            {
                if (pair.count.Value == maxMedalionParts - 1)
                {
                    pair.count.Value = 0;
                    pair.prefab = objectToPickup.GetComponent<PrefabTracker>().Prefab;
                    inventory[controller.Type] = pair;
                    print($"Object {controller.Type} added");
                    objectToPickup.Despawn();
                    MedalionPartsManager.Instance.RemoveAllPartServerRpc();
                    RemoveMedalionsFromOthers();
                    medalions.Value++;
                }
                else
                {
                    pair.count.Value++;
                    pair.prefab = objectToPickup.GetComponent<PrefabTracker>().Prefab;
                    inventory[controller.Type] = pair;
                    print($"Object {controller.Type} added");
                    objectToPickup.Despawn();
                }
            }
            else
            {
                pair.count.Value++;
                pair.prefab = objectToPickup.GetComponent<PrefabTracker>().Prefab;
                inventory[controller.Type] = pair;
                print($"Object {controller.Type} added");
                objectToPickup.Despawn();
            }
        }
    }

    [ServerRpc]
    public void ThrowPickupServerRpc(PickupController.PickupType type, Vector3 force)
    {
        print($"Throw RPC of type {type}");
        if (inventory[type].count.Value > 0)
        {
            inventory[type].count.Value--;
            print($"Object {type} thrown");
            GameObject newPickup = Instantiate(inventory[type].prefab, transform.position, Quaternion.identity);
            PickupController pickupController = newPickup.GetComponent<PickupController>();
            pickupController.NetworkObject.Spawn();
            pickupController.Throw(force);
        }
    }
}
