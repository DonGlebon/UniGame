using UnityEngine;

namespace PlayerContoller
{
    public class PlayerMovement : PhysicalBody
    {
        public float secondJumpForce = 2.5f;

        private bool firstJump = false;
        private bool secondJump = false;
        protected override bool Jump()
        {
            //if (onGround)
            //{
            //    firstJump = false;
            //    secondJump = false;
            //}
            //if (Input.GetKeyDown(KeyCode.Space) && !secondJump)
            //{
            //    if (!firstJump && onGround)
            //    {
            //        firstJump = true;
            //        return true;
            //    }
            //    else
            //    {
            //        secondJump = true;
            //        return true;
            //    }
            //}
            //else
            //    return false;
            return Input.GetKeyDown(KeyCode.Space);
        }
    }

}