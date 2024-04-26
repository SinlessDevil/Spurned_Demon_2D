using System.Linq;
using System.Collections;
using UnityEngine;

namespace Entities.AnimationFSM.StateBehavior
{
    public class AnimJumpState : AnimBehaviorState
    {
        private AnimationClip _animationClip;

        private Coroutine _coroutine;
        
        public override void Enter()
        {
            _animationClip = _anim.runtimeAnimatorController.animationClips.FirstOrDefault(x => x.name == "Jump");
            _anim.SetTrigger(TypeAnimation.IsJump.ToString());

            _coroutine ??= _coroutineService.StartRoutine(HandleAnimationJumpRoutine());
        }

        public override void Exit()
        {
            _anim.SetBool(TypeAnimation.IsJumping.ToString(), false);

            _coroutine = null;
        }

        private IEnumerator HandleAnimationJumpRoutine()
        {
            yield return new WaitForSeconds(_animationClip.length);
            
            _anim.SetBool(TypeAnimation.IsJumping.ToString(), true);
        }
    }
}