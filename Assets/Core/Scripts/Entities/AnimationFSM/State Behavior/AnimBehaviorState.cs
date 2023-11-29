using UnityEngine;

namespace Entities.AnimationFSM.StateBehavior
{
    public abstract class AnimBehaviorState : ICharacterBehavior
    {
        protected Animator _anim;

        public void Init(Animator anim)
        {
            this._anim = anim;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
    }
}