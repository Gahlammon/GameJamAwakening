using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : MonoBehaviour
    {
        private enum PlayerStates
        {
            Throw,
            Death,
            Run,
            Walk,
            Idle
        }
        private bool dead;
        private PlayerStates state;
        private PlayerStates storedState;

        private Animator animator;
        public event System.EventHandler DeadEvent;

        private void Start()
        {
            animator = GetComponent<Animator>();
            state = PlayerStates.Idle;
        }

        private void UseAnimation()
        {
            StateMachine();
        }

        private void StateMachine()
        {
            if (dead)
            {
                if (state == PlayerStates.Death)
                {
                    DeadEvent?.Invoke(this, null);
                    state = PlayerStates.Idle;
                }
                return;
            }
            switch (state)
            {
                case PlayerStates.Idle:
                    animator.Play("BaseLayer.Idle");
                    break;
                case PlayerStates.Death:
                    animator.Play("BaseLayer.Death");
                    break;
                case PlayerStates.Run:
                    animator.Play("BaseLayer.Run");
                    break;
                case PlayerStates.Walk:
                    animator.Play("BaseLayer.Walk");
                    break;
                case PlayerStates.Throw:
                    animator.Play("BaseLayer.Throw");
                    break;
            }
        }

        public void SetThrow()
        {
            if (state != PlayerStates.Throw && state != PlayerStates.Death)
            {
                state = PlayerStates.Throw;
                StateMachine();
            }
        }

        public void SetRun()
        {
            if (state != PlayerStates.Run && state != PlayerStates.Throw && state != PlayerStates.Death)
            {
                state = PlayerStates.Run;
                StateMachine();
            }
            else
            {
                state = PlayerStates.Run;
            }
        }

        public void SetWalk()
        {
            if (state != PlayerStates.Walk && state != PlayerStates.Throw && state != PlayerStates.Death)
            {
                state = PlayerStates.Walk;
                StateMachine();
            }
            else
            {
                state = PlayerStates.Walk;
            }
        }

        public void SetIdle()
        {
            if (state != PlayerStates.Death)
            {
                state = PlayerStates.Idle;
            }
        }

        public void SetDeath()
        {
            state = PlayerStates.Death;
            StateMachine();
        }
    }
}