using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerInputAdapter))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerDeathHandler : MonoBehaviour
{
    private PlayerInputAdapter inputAdapter;
    private PlayerAnimationController animationController;

    private void Awake()
    {
        inputAdapter = GetComponent<PlayerInputAdapter>();
        animationController = GetComponent<PlayerAnimationController>();
    }

    [ClientRpc]
    public void KillPlayerClientRpc()
    {
        print("Player killed");
        inputAdapter.EnableMovement = false;
        gameObject.tag = "Untagged";
        //call death animation
    }
}
