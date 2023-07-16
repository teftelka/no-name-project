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

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }
        }

        private void FixedUpdate()
        {
            var a = 0 - lastHorizontal;
            var b = 0 - horizontal;
            
            Debug.Log(lastHorizontal + "last");
            Debug.Log(horizontal + "new");
            if ((a >= 0 &&  b >=0) || (a <= 0 &&  b <=0))
            {
                controller2D.Move(horizontal * Time.fixedDeltaTime, false, jump);
                jump = false;
            }

        }
    }
}
