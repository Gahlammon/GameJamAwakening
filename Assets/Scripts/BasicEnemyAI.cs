using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicEnemyAI : MonoBehaviour
{
    public float Speed = 0.1f;
    private GameObject[] players;
    private Vector3 target;
    private Rigidbody rb;
    [SerializeField]
    private float maxNoise = 10;
    private float currentNoise = 0;
    private bool awake = false;
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Update()
    {
        
    }

    private void FixedUpdate() 
    {
        if(!awake)
        {
            return;
        }
        Vector3 tmpTarget = Vector3.positiveInfinity;
        foreach (GameObject player in players)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < Vector3.Distance(transform.position, tmpTarget))
            {
                tmpTarget = player.transform.position;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, target, Speed);
    }

    public void IncreaseNoise(float amount)
    {
        currentNoise += amount;
        if(maxNoise < currentNoise)
        {
            awake = true;
        }
    }
}
