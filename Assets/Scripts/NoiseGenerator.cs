using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NoiseGenerator : NetworkBehaviour
{
    public void MakeNoise(float noiseLevel, float range)
    {
        if (IsClient)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("Noise"));
            foreach (Collider collider in hitColliders)
            {
                float noiseToAdd = Mathf.Lerp(noiseLevel, 0, (collider.ClosestPoint(transform.position) - transform.position).magnitude / range);
                collider.gameObject.GetComponent<NoiseListener>().AddNoiseServerRpc(noiseToAdd, transform.position);
            }
        }
    }
}
