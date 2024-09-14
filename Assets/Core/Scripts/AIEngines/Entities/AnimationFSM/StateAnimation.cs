using System;
using System.Collections.Generic;
using Infrastructure.Services.Coroutines;
using UnityEngine;
using Entities.AnimationFSM.StateBehavior;

namespace Entities.AnimationFSM
{
    public class StateAnimation
    {
        private const string AttackTrigger = "TriggerAttack";
        
        private Dictionary<Type, ICharacterBehavior> _behaviorsMap;
        private ICharacterBehavior _behaviorCurrent;

        private Animator _animator;
        private ICoroutineService _coroutineService;
        
        public StateAnimation(Animator animator, ICoroutineService coroutineService)
        {
            _animator = animator;
            _coroutineService = coroutineService;
            
            InitBehaviors();
            SetBehaviorByDefault();
        }
        
        private void InitBehaviors()
        {
            this._behaviorsMap = new Dictionary<Type, ICharacterBehavior>
            {
                [typeof(AnimIdileState)] = new AnimIdileState(),
                [typeof(AnimMoveState)] = new AnimMoveState(),
                [typeof(AnimDeadState)] = new AnimDeadState(),
                [typeof(AnimTakeDamageState)] = new AnimTakeDamageState(),
                [typeof(AnimJumpState)] = new AnimJumpState(),
            };

            foreach (var behavior in _behaviorsMap.Values)
            {
                behavior.Init(_animator, _coroutineService);
            }
        }
        private void SetBehaviorByDefault()
        {
            SetAnimIdile();
        }
        public void UpdateCurrentState()
        {
            _behaviorCurrent.Update();
        }
        
        private void SetBehavior(ICharacterBehavior newBehavior)
        {
            if (this._behaviorCurrent?.GetType() == newBehavior.GetType())
                return;
            
            this._behaviorCurrent?.Exit();
            this._behaviorCurrent = newBehavior;
            this._behaviorCurrent.Enter();
        }
        private ICharacterBehavior GetBehavior<T>() where T : ICharacterBehavior
        {
            var type = typeof(T);
            return this._behaviorsMap[type];
        }

        public void SetAnimIdile()
        {
            var behaviorByDefault = this.GetBehavior<AnimIdileState>();
            this.SetBehavior(behaviorByDefault);
        }
        public void SetAnimMove()
        {
            var behaviorByMove = this.GetBehavior<AnimMoveState>();
            this.SetBehavior(behaviorByMove);
        }
        public void SetAnimAttack()
        {
            _animator.SetTrigger(AttackTrigger);
        }
        public void SetAnimDead()
        {
            var behaviorByDead = this.GetBehavior<AnimDeadState>();
            this.SetBehavior(behaviorByDead);
        }
        public void SetAnimTakeDamage()
        {
            var behaviorByTakeDamage = this.GetBehavior<AnimTakeDamageState>();
            this.SetBehavior(behaviorByTakeDamage);
        }
        public void SetAnimJump()
        {
            var behaviorByJump = this.GetBehavior<AnimJumpState>();
            this.SetBehavior(behaviorByJump);
        }
    }
}