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
        private PlayerThrower playerThrower;

        private float startThrowTimestamp;

        private void Start()
        {
            playerMovement = GetComponent<PlayerMovement>();
            playerThrower = GetComponent<PlayerThrower>();
        }

        public void Move(InputAction.CallbackContext context)
        {

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
                startThrowTimestamp = Time.time;
            }
            if (context.canceled)
            {
                playerThrower.Throw(Time.time - startThrowTimestamp);
            }
        }
    }
}