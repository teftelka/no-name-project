using System;
using UnityEngine;

namespace PlayerScripts
{
    public class AnimationController : MonoBehaviour
    {
        private Animator _animator;


        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void SetAnimationMovement(float movement, bool isGrounded)
        {
            if (movement > 0.1f || movement < -0.1f)
            {
                _animator.SetBool("Running", true);
            }
            else
            {
                _animator.SetBool("Running", false);
            }

            if (!isGrounded)
            {
                _animator.SetBool("Running", false);
                _animator.SetBool("Jumping", true);
            }
            else
            {
                _animator.SetBool("Jumping", false);
            }
        }

        public void SetAttackAnimation(int weaponId)
        {
            _animator.SetTrigger("Attack");
        }
    }
}
