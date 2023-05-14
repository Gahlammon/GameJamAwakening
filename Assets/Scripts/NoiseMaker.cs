using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NoiseGenerator))]
[RequireComponent(typeof(AudioSource))]
public class NoiseMaker : MonoBehaviour
{
    [SerializeField]
    private float noiseLevel = 50;
    [SerializeField]
    private float noiseRange = 10;
    private NoiseGenerator noiseGenerator;
    private AudioSource source;
    private void Start()
    {
        noiseGenerator = GetComponent<NoiseGenerator>();
        source = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision other)
    {
        noiseGenerator.MakeNoise(noiseLevel, noiseRange);
        source.Play();
    }
}
