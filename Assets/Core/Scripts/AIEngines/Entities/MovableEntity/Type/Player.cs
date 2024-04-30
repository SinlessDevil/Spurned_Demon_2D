using Core.Scripts.AIEngines.Healths;
using Infrastructure.Services.Coroutines;
using UnityEngine;
using Entities.AnimationFSM;
using Extensions;
using Zenject;

namespace Entities.MovableEntity.Type
{
    public class Player : MobileEntity
    {
        [Space(10)] [Header("Presenters")]
        [SerializeField] private Health _health;
        [Space(10)] [Header("Viewer")]
        [SerializeField] private PlayerHealthViewer _playerHealthViewer;
        
        private StateAnimation _stateAnimation;
        private Animator _animator;

        private ICoroutineService _coroutineService;
        
        [Inject]
        public void Constructor(ICoroutineService coroutineService)
        {
            _coroutineService = coroutineService;
        }

        public override void Initialize()
        {
            base.Initialize();
            _health.Initialize(100);
            _playerHealthViewer.Initialize(_health);
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