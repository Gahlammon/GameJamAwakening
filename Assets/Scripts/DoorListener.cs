using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DoorListener : MonoBehaviour
{
    [ServerRpc]
    public void DestroyDoorRpc()
    {
        Destroy(gameObject);
    }
}
