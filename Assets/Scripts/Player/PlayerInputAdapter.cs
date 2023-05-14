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
        [Header("Config")]
        [SerializeField]
        private float deadzone = 0.1f;

        public event System.EventHandler ThrowStartEvent;
        public event System.EventHandler ThrowEndEvent;
        public float MaxHoldTime => playerThrower.MaxHoldTime;

        private bool enableMovement = true;

        private PlayerMovement playerMovement;
        private PlayerThrower playerThrower;
        private PlayerAimer playerAimer;
        private PlayerInventory playerInventory;

        private float startThrowTimestamp;

        public bool EnableMovement
        {
            get => enableMovement; set
            {
                enableMovement = value;
                playerMovement.SetMoveDirection(Vector2.zero);
            }
        }

        private void Start()
        {
            playerMovement = GetComponent<PlayerMovement>();
            playerThrower = GetComponent<PlayerThrower>();
            playerAimer = GetComponent<PlayerAimer>();
            playerInventory = GetComponent<PlayerInventory>();
        }

        public void Move(InputAction.CallbackContext context)
        {
            if (EnableMovement)
            {
                playerMovement.SetMoveDirection(ApplyDeadzoneToInput(context.ReadValue<Vector2>()));
            }
        }

        public void Aim(InputAction.CallbackContext context)
        {
            if (EnableMovement)
            {
                playerAimer.SetAimSpeed(ApplyDeadzoneToInput(context.ReadValue<Vector2>()));
            }
        }

        public void Run(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (EnableMovement)
                {
                    playerMovement.IsSprinting = true;
                }
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
                ThrowStartEvent?.Invoke(this, null);
                startThrowTimestamp = Time.time;
            }
            if (context.canceled)
            {
                ThrowEndEvent?.Invoke(this, null);
                if (EnableMovement)
                {
                    playerThrower.Throw(Time.time - startThrowTimestamp);
                }
            }
        }

        public void PreviousItem(InputAction.CallbackContext context)
        {
            if (EnableMovement)
            {
                if (context.started)
                {
                    playerInventory.ChangeIndex(-1);
                }
            }
        }

        public void NextItem(InputAction.CallbackContext context)
        {
            if (EnableMovement)
            {
                if (context.started)
                {
                    playerInventory.ChangeIndex(1);
                }
            }
        }

        private Vector2 ApplyDeadzoneToInput(Vector2 input)
        {
            if (input.magnitude < deadzone)
            {
                return Vector2.zero;
            }
            return input.normalized * ((input.magnitude - deadzone) / (1 - deadzone));
        }
    }
}