using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NoiseListener : NetworkBehaviour
{
    [SerializeField]
    private float maxNoise;

    private NetworkVariable<float> currentNoise = new NetworkVariable<float>();

    public NetworkVariable<float> CurrentNoise => currentNoise;

    [ServerRpc]
    public void AddNoiseServerRpc(float noise, Vector3 sourcePosition)
    {
        currentNoise.Value += noise;
        if (currentNoise.Value >= maxNoise)
        {
            currentNoise.Value = maxNoise;
            print($"Object awaken: {gameObject.name}");
            //call event
        }
    }
}
