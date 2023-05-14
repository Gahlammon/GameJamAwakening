using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Collider))]
public class NoiseListener : NetworkBehaviour
{
    [SerializeField]
    private float noiseToAwake;

    [ReadOnly]
    [SerializeField]
    private float currentNoise = 0;

    public bool Awaken { get; private set; } = false;

    public event EventHandler<(Vector3 position, float value)> NoiseHeardEvent;
    public event EventHandler<bool> AwakeStateChanged;
    public event EventHandler<float> NoiseIncreasedEvent;

    private void Awake()
    {
        if (IsClient)
        {
            enabled = false;
        }
        if (currentNoise > noiseToAwake)
        {
            Awaken = true;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddNoiseServerRpc(float noise, Vector3 sourcePosition)
    {
        currentNoise += noise;
        if (currentNoise >= noiseToAwake)
        {
            currentNoise = noiseToAwake;
            if (!Awaken)
            {
                print($"Object awaken: {gameObject.name}");
                Awaken = true;
                AwakeStateChanged?.Invoke(this, true);
            }
            NoiseHeardEvent?.Invoke(this, (sourcePosition, noise));
        }
        NoiseIncreasedEvent?.Invoke(this, currentNoise / noiseToAwake);
    }
}
