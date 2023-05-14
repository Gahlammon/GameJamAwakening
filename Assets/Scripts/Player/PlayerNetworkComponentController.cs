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
    [RequireComponent(typeof(PlayerInputAdapter))]
    [RequireComponent(typeof(ServerInventory))]
    [RequireComponent(typeof(PlayerAimer))]
    public class PlayerNetworkComponentController : NetworkBehaviour
    {
        [Header("Config")]
        [SerializeField]
        private List<RuntimeAnimatorController> animatorControllers = new List<RuntimeAnimatorController>();

        [Header("References")]
        [SerializeField]
        private GameObject pickupColliderGameObject;
        [SerializeField]
        private Animator animator;

        public int Id { get => id.Value; set => id.Value = value; }

        private NetworkVariable<int> id = new NetworkVariable<int>();

        private CapsuleCollider capsuleCollider;
        private CharacterController characterController;
        private PlayerInput playerInput;
        private PlayerMovement playerMovement;
        private PlayerInventory playerInventory;
        private PlayerInputAdapter playerInputAdapter;
        private ServerInventory serverInventory;
        private PlayerAimer playerAimer;

        private void Awake()
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
            characterController = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            playerMovement = GetComponent<PlayerMovement>();
            playerInventory = GetComponent<PlayerInventory>();
            playerInputAdapter = GetComponent<PlayerInputAdapter>();
            serverInventory = GetComponent<ServerInventory>();
            playerAimer = GetComponent<PlayerAimer>();

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
            StartCoroutine(OnNetworkSpawnCoroutine());
        }

        private IEnumerator OnNetworkSpawnCoroutine()
        {
            yield return null;
            yield return null;
            if (!IsOwner)
            {
                enabled = false;
                characterController.enabled = false;
                capsuleCollider.enabled = true;
                pickupColliderGameObject.SetActive(false);
            }
            else
            {
                CameraPlayerFollower.Instance.SetPlayerTransform(transform);
                UI.UIInventoryWheel.Instance.SetPlayerInventory(playerInventory);
                UI.UIThrowController.Instance.SetPlayerInputAdapter(playerInputAdapter);
                UI.UICrosshair.Instance.SetPlayerAimer(playerAimer);
                playerMovement.enabled = true;
                playerInput.enabled = true;
                pickupColliderGameObject.SetActive(true);
                characterController.enabled = true;
            }
            yield return null;
            animator.runtimeAnimatorController = animatorControllers[Id - 1];
            UI.UIStats.Instance.RegisterServerInventory(serverInventory, Id);
        }
    }
}