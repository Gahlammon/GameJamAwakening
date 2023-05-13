using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerThrower))]
    [RequireComponent(typeof(PlayerAimer))]
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerInputAdapter : MonoBehaviour
    {
        private PlayerMovement playerMovement;
        private PlayerThrower playerThrower;
        private PlayerAimer playerAimer;
        private PlayerInventory playerInventory;

        private float startThrowTimestamp;

        private void Start()
        {
            playerMovement = GetComponent<PlayerMovement>();
            playerThrower = GetComponent<PlayerThrower>();
            playerAimer = GetComponent<PlayerAimer>();
            playerInventory = GetComponent<PlayerInventory>();
        }

        public void Move(InputAction.CallbackContext context)
        {
            playerMovement.SetMoveDirection(context.ReadValue<Vector2>());
        }

        public void Aim(InputAction.CallbackContext context)
        {
            playerAimer.SetAimSpeed(context.ReadValue<Vector2>());
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
                startThrowTimestamp = Time.time;
            }
            if (context.canceled)
            {
                playerThrower.Throw(Time.time - startThrowTimestamp);
            }
        }

        public void PreviousItem(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                playerInventory.ChangeIndex(-1);
            }
        }

        public void NextItem(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                playerInventory.ChangeIndex(1);
            }
        }
    }
}