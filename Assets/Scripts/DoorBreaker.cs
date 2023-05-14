using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DoorBreaker : NetworkBehaviour
{
    public void BreakDoors(float range)
    {
        if (IsClient)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("Doors"));
            foreach (Collider collider in hitColliders)
            {
                collider.gameObject.GetComponent<DoorListener>().DestroyDoorRpc();
            }
        }
    }
}

