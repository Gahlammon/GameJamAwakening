using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(Rigidbody))]
public class BasicEnemyAI : MonoBehaviour
{
    public float Speed = 1;
    private GameObject[] players;
    private Vector3 target;
    private Rigidbody rb;
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        rb = GetComponent<Rigidbody>();
        Speed *= 0.1f;
    }

    void Update()
    {
        
    }

    private void FixedUpdate() 
    {
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
}
