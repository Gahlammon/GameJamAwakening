using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NoiseGenerator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PickupController))]
public class NoiseMaker : MonoBehaviour
{
    [SerializeField]
    private float noiseLevel = 50;
    [SerializeField]
    private float noiseRange = 10;
    private NoiseGenerator noiseGenerator;
    private PickupController pickupController;
    private AudioSource source;
    private void Start()
    {
        noiseGenerator = GetComponent<NoiseGenerator>();
        source = GetComponent<AudioSource>();
        pickupController = GetComponent<PickupController>();
        pickupController.OnCollisionEvent += (_,_) => OnCollision();
        pickupController.OnEnemyCollisionEvent += (_, _) => OnCollision();
    }
    private void OnCollision()
    {
        noiseGenerator.MakeNoise(noiseLevel, noiseRange);
        AudioSource.PlayClipAtPoint(source.clip, transform.position);
    }
}
