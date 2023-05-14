using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Explode : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void StartExplode()
    {
        animator.enabled = true;
        spriteRenderer.size = new Vector2(5, 5);
        animator.Play("BaseLayer.Explode");
        AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
    }

    public void EndExplode()
    {
        Destroy(gameObject);
    }

}
