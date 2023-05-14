using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Explode : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartExplode()
    {
        animator.enabled = true;
        spriteRenderer.size = new Vector2(5, 5);
        animator.Play("BaseLayer.Explode");
    }

    public void EndExplode()
    {
        Destroy(gameObject);
    }

}
