using Entities.AnimationFSM;
using Extensions;
using UnityEngine;

namespace Entities.MovableEntity.Type
{
    public class Player : MobileEntity
    {
        private StateAnimation _stateAnimation;
        private Animator _animator;
        
        public override bool IsMoving
        {
            set
            {
                if (value == true)
                {
                    _stateAnimation.SetAnimMove();
                }
                else
                {
                    _stateAnimation.SetAnimIdile();
                }
            }
        }
        
        public override bool IsJumping
        {
            set
            {
                if (value == true)
                {
                    _stateAnimation.SetAnimJump();
                }
                else
                {
                    _stateAnimation.SetAnimIdile();
                }
            }
        }
        
        public void InitConfig(float moveSpeed, float jumpHeight)
        {
            _moveSpeed = moveSpeed;
            _jumpHeight = jumpHeight;
        }

        protected override void InitComponent()
        {
            _animator = GetComponent<Animator>();
            _stateAnimation = new(_animator);
        }
        protected override void Asserts()
        {
            _animator.LogErrorIfComponentNull();
            _stateAnimation.LogErrorIfComponentNull();
        }
    }
}