using System;
using System.Collections.Generic;
using Entities.AnimationFSM.StateBehavior;
using UnityEngine;

namespace Entities.AnimationFSM
{
    public class StateAnimation
    {
        private Dictionary<Type, ICharacterBehavior> _behaviorsMap;
        private ICharacterBehavior _behaviorCurrent;

        private Animator _animator;

        public StateAnimation(Animator animator)
        {
            InitAnimator(animator);
            InitBehaviors();
            SetBehaviorByDefault();
        }
        private void InitAnimator(Animator animator)
        {
            _animator = animator;
        }
        private void InitBehaviors()
        {
            this._behaviorsMap = new Dictionary<Type, ICharacterBehavior>
            {
                [typeof(AnimIdileState)] = new AnimIdileState(),
                [typeof(AnimMoveState)] = new AnimMoveState(),
                [typeof(AnimAttackState)] = new AnimAttackState(),
                [typeof(AnimDeadState)] = new AnimDeadState(),
                [typeof(AnimTakeDamageState)] = new AnimTakeDamageState(),
                [typeof(AnimJumpState)] = new AnimJumpState(),
            };

            foreach (var behavior in _behaviorsMap.Values)
            {
                behavior.Init(_animator);
            }
        }

        private void SetBehavior(ICharacterBehavior newBehavior)
        {
            if (this._behaviorCurrent?.GetType() == newBehavior.GetType())
                return;
            
            this._behaviorCurrent?.Exit();
            this._behaviorCurrent = newBehavior;
            this._behaviorCurrent.Enter();
        }
        private void SetBehaviorByDefault(){
            SetAnimIdile();
        }
        private ICharacterBehavior GetBehavior<T>() where T : ICharacterBehavior
        {
            var type = typeof(T);
            return this._behaviorsMap[type];
        }

        public void SetAnimIdile(){
            var behaviorByDefault = this.GetBehavior<AnimIdileState>();
            this.SetBehavior(behaviorByDefault);
        }
        public void SetAnimMove(){
            var behaviorByMove = this.GetBehavior<AnimMoveState>();
            this.SetBehavior(behaviorByMove);
        }
        public void SetAnimAttack()
        {
            var behaviorByAttack = this.GetBehavior<AnimAttackState>();
            this.SetBehavior(behaviorByAttack);
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