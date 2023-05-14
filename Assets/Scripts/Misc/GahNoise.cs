using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(NoiseListener))]
public class GahNoise : NetworkBehaviour
{
    private NetworkVariable<float> percentNoise = new NetworkVariable<float>(0);
    private NoiseListener noiseListener;

    private void Start()
    {
        noiseListener = GetComponent<NoiseListener>();
        noiseListener.NoiseIncreasedEvent += (_, e) => percentNoise.Value = e;
        percentNoise.OnValueChanged += (_, e) => UI.UIGahSound.Instance.UpdateBar(e);
    }
}
