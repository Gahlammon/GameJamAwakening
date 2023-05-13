using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

namespace Player
{
    [DefaultExecutionOrder(1)]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerNetworkComponentController : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameObject pickupColliderGameObject;

        private CapsuleCollider capsuleCollider;
        private CharacterController characterController;
        private PlayerInput playerInput;
        private PlayerMovement playerMovement;

        private void Awake()
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
            characterController = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            playerMovement = GetComponent<PlayerMovement>();

            capsuleCollider.enabled = false;
            characterController.enabled = false;
            playerInput.enabled = false;
            playerMovement.enabled = false;

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

            playerMovement.enabled = true;
            playerInput.enabled = true;
            pickupColliderGameObject.SetActive(true);
            characterController.enabled = true;
        }
    }
}