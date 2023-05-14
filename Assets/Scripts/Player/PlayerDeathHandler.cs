using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerInputAdapter))]
public class PlayerDeathHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerAnimationController animationController;

    private PlayerInputAdapter inputAdapter;
    private bool dead = false;

    private void Awake()
    {
        inputAdapter = GetComponent<PlayerInputAdapter>();
    }

    [ClientRpc]
    public void KillPlayerClientRpc()
    {
        if (!dead)
        {
            print("Player killed");
            inputAdapter.EnableMovement = false;
            gameObject.tag = "Untagged";
            animationController.SetDeath();
            dead = true;
        }
    }
}
