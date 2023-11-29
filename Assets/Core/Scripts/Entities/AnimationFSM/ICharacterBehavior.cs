using UnityEngine;

namespace Entities.AnimationFSM
{
    public interface ICharacterBehavior
    {
        void Init(Animator anim);
        void Enter();
        void Exit();
    }
}