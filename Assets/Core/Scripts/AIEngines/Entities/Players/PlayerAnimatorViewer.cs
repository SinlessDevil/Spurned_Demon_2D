using System;
using Infrastructure.Services.Coroutines;
using UnityEngine;
using Entities.AnimationFSM;
using Extensions;
using JetBrains.Annotations;
using Zenject;

namespace Core.Scripts.AIEngines.Entities.Players
{
    public class PlayerAnimatorViewer : MonoBehaviour
    {
        private PlayerMover _playerMover;
        
        private StateAnimation _stateAnimation;
        private Animator _animator;

        private ICoroutineService _coroutineService;

        [Inject]
        public void Constructor(ICoroutineService coroutineService)
        {
            _coroutineService = coroutineService;
        }

        public event Action<string> TriggeredAttackEvent;
        
        public void Initialize(PlayerMover playerMover)
        {
            _playerMover = playerMover;
            _animator = GetComponent<Animator>();
            _stateAnimation = new(_animator, _coroutineService);
            Asserts();
        }

        public void Dispose()
        {
            _playerMover = null;
            _animator = null;
            _stateAnimation = null;
        }
        

        public void PlayMovingAnimation(bool isMoving)
        {
            if (isMoving)
                _stateAnimation.SetAnimMove();
            else
                _stateAnimation.SetAnimIdile();
        }
        public void PlayJumpingAnimation(bool isJumping)
        {
            if (isJumping == false)
                _stateAnimation.SetAnimJump();
            else
                _stateAnimation.SetAnimIdile();
        }
        public void PlayAttackAnimation()
        {
            _stateAnimation.SetAnimAttack();
        }
        
                
        private void Update()
        {
            _stateAnimation.UpdateCurrentState();
        }
        
        [UsedImplicitly]
        public void Trigger(string eventType)
        {
            TriggeredAttackEvent?.Invoke(eventType);
        }
        
        private void Asserts()
        {
            _animator.LogErrorIfComponentNull();
            _stateAnimation.LogErrorIfComponentNull();
            _playerMover.LogErrorIfComponentNull();
        }
    }   
}