using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ServerInventory : NetworkBehaviour
{
    public NetworkVariable<int> Pipes => pipes;
    public NetworkVariable<int> Bricks => bricks;
    public NetworkVariable<int> Bootles => bootles;
    public NetworkVariable<int> Knifes => knifes;

    private NetworkVariable<int> pipes = new NetworkVariable<int>(0);
    private NetworkVariable<int> bricks = new NetworkVariable<int>(0);
    private NetworkVariable<int> bootles = new NetworkVariable<int>(0);
    private NetworkVariable<int> knifes = new NetworkVariable<int>(0);

    private Dictionary<PickupController.PickupType, (GameObject prefab, NetworkVariable<int> count)> inventory;

    private void Awake()
    {
        inventory = new Dictionary<PickupController.PickupType, (GameObject prefab, NetworkVariable<int> count)>
        {
            { PickupController.PickupType.Bootle, (null, bootles) },
            { PickupController.PickupType.Pipe, (null, pipes) },
            { PickupController.PickupType.Brick, (null, bricks) },
            { PickupController.PickupType.Knife, (null, pipes) }
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

    [ServerRpc]
    public void PickupPickupServerRpc(ulong objectId)
    {
        print("Pickup RPC");
        NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objectId, out NetworkObject objectToPickup);
        if (objectToPickup != null)
        {
            PickupController controller = objectToPickup.GetComponent<PickupController>();
            (GameObject prefab, NetworkVariable<int> count) pair = inventory[controller.Type];
            pair.count.Value++;
            pair.prefab = objectToPickup.GetComponent<PrefabTracker>().Prefab;
            inventory[controller.Type] = pair;
            print($"Object {controller.Type} added");
            objectToPickup.Despawn();
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
