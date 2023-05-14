using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(NoiseGenerator))]
    [RequireComponent(typeof(PlayerAnimationController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Speed")]
        [SerializeField]
        private float walkSpeed = 5;
        [SerializeField]
        private float sprintSpeed = 10;

        [Header("Noise")]
        [SerializeField]
        private float walkNoiseRange = 2;
        [SerializeField]
        private float walkNoiseLevel = 1;
        [SerializeField]
        private float sprintNoiseRange = 5;
        [SerializeField]
        private float sprintNoiseLevel = 5;

        [HideInInspector]
        public bool IsSprinting = false;
        private CharacterController characterController;
        private NoiseGenerator noiseGenerator;
        private PlayerAnimationController animationController;

        private Vector3 moveDirection = Vector3.zero;
        private float speed => IsSprinting ? sprintSpeed : walkSpeed;

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            noiseGenerator = GetComponent<NoiseGenerator>();
            animationController = GetComponent<PlayerAnimationController>();
        }

        private void FixedUpdate()
        {
            characterController.SimpleMove(moveDirection * speed);
            if(moveDirection != Vector3.zero)
            {
                if(IsSprinting)
                {
                    animationController.SetRun();
                }
                else
                {
                    animationController.SetWalk();
                }
            }
            else
            {
                animationController.SetIdle();
            }
            float noiseValue = (IsSprinting ? sprintNoiseLevel : walkNoiseLevel) * Time.deltaTime * moveDirection.magnitude;
            noiseGenerator.MakeNoise(noiseValue, IsSprinting ? sprintNoiseRange : walkNoiseRange);
        }

        public void SetMoveDirection(Vector2 direction)
        {
            moveDirection = new Vector3(direction.x, 0, direction.y);
        }
    }
}
