using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField]
        private float walkSpeed = 5;
        [SerializeField]
        private float sprintSpeed = 10;

        [HideInInspector]
        public bool IsSprinting = false;

        private CharacterController characterController;

        private Vector3 moveDirection = Vector3.zero;
        private float speed => IsSprinting ? sprintSpeed : walkSpeed; 

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            characterController.SimpleMove(moveDirection * speed);
        }

        public void SetMoveDirection(Vector2 direction)
        {
            moveDirection = new Vector3(direction.x, 0, direction.y);
        }
    }
}
