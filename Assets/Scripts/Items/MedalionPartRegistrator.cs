using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MedalionPartRegistrator : NetworkBehaviour
{
    private NetworkObjectReference reference;

    private void Start()
    {
        if (IsOwner)
        {
            reference = new NetworkObjectReference(NetworkObject);
            MedalionPartsManager.Instance.RegisterPartServerRpc(reference);
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        if (IsOwner)
        {
            MedalionPartsManager.Instance?.UnregisterPartServerRpc(reference);
        }
    }
}
