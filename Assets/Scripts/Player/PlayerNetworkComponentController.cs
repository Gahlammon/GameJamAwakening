using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(1)]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerNetworkComponentController : NetworkBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject pickupColliderGameObject;

    private CapsuleCollider capsuleCollider;
    private CharacterController characterController;
    private PlayerInput playerInput;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        capsuleCollider.enabled = false;
        characterController.enabled = false;
        playerInput.enabled = false;

        pickupColliderGameObject.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        enabled = IsClient;
        if (!IsOwner)
        {
            enabled = false;
            characterController.enabled = false;
            capsuleCollider.enabled = true;
            pickupColliderGameObject.SetActive(false);
            return;
        }

        playerInput.enabled = true;
        pickupColliderGameObject.SetActive(true);
        characterController.enabled = true;
    }
}
