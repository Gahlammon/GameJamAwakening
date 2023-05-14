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
    [RequireComponent(typeof(PlayerInventory))]
    [RequireComponent(typeof(Animator))]
    public class PlayerNetworkComponentController : NetworkBehaviour
    {
        [Header("Config")]
        [SerializeField]
        private List<RuntimeAnimatorController> animatorControllers = new List<RuntimeAnimatorController>();

        [Header("References")]
        [SerializeField]
        private GameObject pickupColliderGameObject;

        public int Id { get => id.Value; set => id.Value = value; }

        private NetworkVariable<int> id = new NetworkVariable<int>();

        private CapsuleCollider capsuleCollider;
        private CharacterController characterController;
        private PlayerInput playerInput;
        private PlayerMovement playerMovement;
        private PlayerInventory playerInventory;
        private Animator animator;

        private void Awake()
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
            characterController = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            playerMovement = GetComponent<PlayerMovement>();
            playerInventory = GetComponent<PlayerInventory>();
            animator = GetComponent<Animator>();

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
            StartCoroutine(UpdateAnimatorCoroutine());
            if (!IsOwner)
            {
                enabled = false;
                characterController.enabled = false;
                capsuleCollider.enabled = true;
                pickupColliderGameObject.SetActive(false);
                return;
            }

            CameraPlayerFollower.Instance.SetPlayerTransform(transform);
            UI.UIInventoryWheel.Instance.SetPlayerInventory(playerInventory);
            playerMovement.enabled = true;
            playerInput.enabled = true;
            pickupColliderGameObject.SetActive(true);
            characterController.enabled = true;
        }

        private IEnumerator UpdateAnimatorCoroutine()
        {
            yield return null;
            yield return null;
            animator.runtimeAnimatorController = animatorControllers[Id - 1];
        }
    }
}