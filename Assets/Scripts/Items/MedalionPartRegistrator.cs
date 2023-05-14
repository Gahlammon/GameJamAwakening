using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MedalionPartRegistrator : NetworkBehaviour
{
    private NetworkObjectReference reference;

    private void Start()
    {
        reference = new NetworkObjectReference(NetworkObject);
        MedalionPartsManager.Instance.RegisterPartServerRpc(reference);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        MedalionPartsManager.Instance.UnregisterPartServerRpc(reference);
    }
}
