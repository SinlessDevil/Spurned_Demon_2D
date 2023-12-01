using Infrastructure.Services.Coroutines;
using UnityEngine;

namespace Entities.AnimationFSM.StateBehavior
{
    public abstract class AnimBehaviorState : ICharacterBehavior
    {
        protected Animator _anim;
        protected ICoroutineService _coroutineService;
        
        public void Init(Animator anim, ICoroutineService coroutineService)
        {
            _anim = anim;
            _coroutineService = coroutineService;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
    }
}