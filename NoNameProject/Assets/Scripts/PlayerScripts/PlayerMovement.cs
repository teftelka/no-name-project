using UnityEngine;

namespace PlayerScripts
{
    public class PlayerMovement : MonoBehaviour
    {
        public CharacterController2D controller2D;
        [SerializeField]private float runSpeed = 40f;
        private bool jump;
        private float horizontal;
        private float lastHorizontal;

        private void Update()
        {
            lastHorizontal = horizontal;
            horizontal = Input.GetAxisRaw("Horizontal") * runSpeed;
            
        }

        public void SetJump()
        {
            jump = true;
        }

        private void FixedUpdate()
        {
            var a = 0 - lastHorizontal;
            var b = 0 - horizontal;
            
            if ((a >= 0 &&  b >=0) || (a <= 0 &&  b <=0))
            {
                controller2D.Move(horizontal * Time.fixedDeltaTime, false, jump);
                jump = false;
            }

        }
    }
}
