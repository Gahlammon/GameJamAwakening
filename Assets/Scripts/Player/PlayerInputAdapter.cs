using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerInputAdapter : MonoBehaviour
    {
        private PlayerMovement playerMovement;

        private void Start()
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        public void Move(InputAction.CallbackContext context)
        {
            playerMovement.SetMoveDirection(context.ReadValue<Vector2>());
        }

        public void Aim(InputAction.CallbackContext context)
        {
            print("Aim: " + context.ReadValue<Vector2>().ToString());
        }

        public void Run(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                playerMovement.IsSprinting = true;
            }
            else if (context.canceled)
            {
                playerMovement.IsSprinting = false;
            }
        }

        public void Throw(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                print("Start throw");
            }
            else if (context.canceled)
            {
                print("Stop throw");
            }
        }
    }
}