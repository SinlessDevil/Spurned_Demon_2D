using System;
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
            float moveInput = Input.GetAxis("Horizontal");
            if (moveInput > 0)
            {
                _controllable.MoveTo(moveInput);
                _controllable.FlipBody(moveInput);
                
                _controllable.IsMoving = true;
            }

            if (moveInput == 0 && _controllable.IsMoving == true)
            {
                _controllable.IsMoving = false;
            }
        }
        private void InputMoveLeft()
        {
            float moveInput = Input.GetAxis("Horizontal");
            if (moveInput < 0)
            {
                _controllable.MoveTo(moveInput);
                _controllable.FlipBody(moveInput);
                
                _controllable.IsMoving = true;
            }
            
            if (moveInput == 0 && _controllable.IsMoving == true)
            {
                _controllable.IsMoving = false;
            }
        }
        private void InputJump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _controllable.IsJumping = true;

                if (_controllable.IsJumping == true && _controllable.IsGround == true)
                {
                    _controllable.Jump();
                }

                _controllable.IsJumping = false;
            }
        }
    }
}