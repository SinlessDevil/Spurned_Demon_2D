using System;
using Entities.AnimationFSM;
using Extensions;
using Infrastructure.Services.Coroutines;
using UnityEngine;
using Zenject;

namespace Entities.MovableEntity.Type
{
    public class Player : MobileEntity
    {
        private StateAnimation _stateAnimation;
        private Animator _animator;

        private ICoroutineService _coroutineService;
        
        [Inject]
        public void Constructor(ICoroutineService coroutineService)
        {
            _coroutineService = coroutineService;
        }
        
        public override bool IsMoving
        {
            set
            {
                _isMoving = value;
                
                if(_isGround == false)
                    return;
                
                if (_isMoving == true)
                {
                    _stateAnimation.SetAnimMove();
                }
                else
                {
                    _stateAnimation.SetAnimIdile();
                }
            }
        }
        public override bool IsGround
        {
            set
            {
                _isGround = value;
                if (_isGround == false)
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
            base.InitComponent();
            _animator = GetComponent<Animator>();
            _stateAnimation = new(_animator, _coroutineService);
        }
        protected override void Asserts()
        {
            base.Asserts();
            _animator.LogErrorIfComponentNull();
            _stateAnimation.LogErrorIfComponentNull();
        }

        private void Update()
        {
            _stateAnimation.UpdateCurrentState();
        }
    }
}