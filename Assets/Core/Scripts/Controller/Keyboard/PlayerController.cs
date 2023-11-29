using UnityEngine;

namespace Controller.Keyboard
{
    public class PlayerController : MonoBehaviour
    {
        private bool _isInitialize;

        private IConrollable _controllable;

        public void Initialize(IConrollable conrollable)
        {
            _controllable = conrollable;

            _isInitialize = true;
        }

        public void Update()
        {
            if (_isInitialize == false)
                return;

            InputMoveLeft();

            InputMoveRight();

            InputJump();
        }

        private void InputMoveRight()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                _controllable.MoveTo(1);
                _controllable.FlipBody(1);
            }
        }
        private void InputMoveLeft()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _controllable.MoveTo(-1);
                _controllable.FlipBody(-1);
            }
        }
        private void InputJump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _controllable.IsJumping = true;

                if (_controllable.IsJumping == true && _controllable.IsGound == true)
                {
                    _controllable.Jump();
                }

                _controllable.IsJumping = false;
            }
        }
    }
}