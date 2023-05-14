using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MedalionPartsManager : NetworkSingleton<MedalionPartsManager>
{
    private HashSet<NetworkObjectReference> parts = new HashSet<NetworkObjectReference>();

    [ServerRpc]
    public void RegisterPartServerRpc(NetworkObjectReference part)
    {
        parts.Add(part);
    }

    [ServerRpc]
    public void UnregisterPartServerRpc(NetworkObjectReference part)
    {
        parts.Remove(part);
    }

    [ServerRpc]
    public void RemoveAllPartServerRpc()
    {
        foreach (NetworkObjectReference part in parts)
        {
            if (part.TryGet(out NetworkObject networkObject))
            {
                networkObject.Despawn();
            }
        }
        parts.Clear();
    }
}
