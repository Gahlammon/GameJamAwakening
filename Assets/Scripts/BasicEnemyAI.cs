using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : MonoBehaviour
{
    GameObject[] players;
    Vector3 target;
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        
        foreach (GameObject player in players)
        {

        }
    }
}
