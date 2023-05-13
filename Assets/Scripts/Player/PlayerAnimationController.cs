using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : MonoBehaviour
    {
        private bool toThrow = false;
        private bool death = false;
        private bool run = false;
        private bool walk = false;
        
        private Animator animator;

        private void Start() 
        {
            animator = GetComponent<Animator>();
        }

        private void UseAnimation()
        {
            if(death)
            {
                animator.Play("BaseLayer.Death");
                death = false;
            }
            else if(toThrow)
            {
                animator.Play("BaseLayer.Throw");
                toThrow = false;
            }
            else if(run)
            {
                animator.Play("BaseLayer.Run");
                run = false;
            }
            else if(walk)
            {
                animator.Play("BaseLayer.Walk");
                walk = false;
            }
            else
            {
                animator.Play("BaseLayer.Idle");
            }
        }

        public void SetThrow()
        {
            toThrow = true;
        }

        public void SetRun(bool on)
        {
            run = on;
        }

        public void SetWalk(bool on)
        {
            walk = on;
        }
    }
}