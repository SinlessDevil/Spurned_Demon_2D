using System;
using Controller.Keyboard;
using Controllers;
using Entities.MovableEntity.Type;
using Infrastructure.Services.Input;
using Infrastructure.Services.PlayerServices;
using Infrastructure.Services.StaticData;
using UnityEngine;
using Zenject;

namespace GameController
{
    public class PlayerMoveController : IGameController, ITickable
    { 
        private const float SmoothingFactor = 10;
        
        private readonly IInputService _inputService;
        private readonly IPlayerService _playerService;
        private readonly IStaticDataService _staticDataService;

        private Vector3 _currentDirection;
        private IConrollable _controllable;

        public PlayerMoveController(
            IInputService inputService,
            IPlayerService playerService,
            IStaticDataService staticDataService)
        {
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            _playerService = playerService ?? throw new ArgumentNullException(nameof(playerService));
            _staticDataService = staticDataService;
        }
        public void Activate()
        {
            _playerService.OnPlayerAdded += TrySetMover;
            _playerService.OnPlayerRemoved += RemoveMover;
        }

        public void Deactivate()
        {
            _controllable = null; 
            
            _playerService.OnPlayerAdded -= TrySetMover;
            _playerService.OnPlayerRemoved -= RemoveMover;
        }

        void ITickable.Tick()
        {
            if (_controllable != null)
            {
                InputMoveRight();
                InputMoveLeft();
                InputJump();
            }
        }

        private void InputMoveRight()
        {
            float moveInput = _inputService.Horizontal;
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
            float moveInput = _inputService.Horizontal;
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
            float moveInput = _inputService.Vertical;
            if (moveInput > 0.5f)
            {
                _controllable.IsJumping = true;

                if (_controllable.IsJumping == true && _controllable.IsGround == true)
                {
                    _controllable.Jump();
                }

                _controllable.IsJumping = false;
            }
        }

        private void TrySetMover(Player player)
        {
            _controllable = player;
            Debug.LogWarning("Added " + _controllable);
        }

        private void RemoveMover(Player player)
        {
            _controllable = null;
            Debug.LogWarning("Removed " + _controllable);
        }
    }
}