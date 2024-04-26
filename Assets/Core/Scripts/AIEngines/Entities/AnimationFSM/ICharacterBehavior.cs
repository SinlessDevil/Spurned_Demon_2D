using Infrastructure.Services.Coroutines;
using UnityEngine;

namespace Entities.AnimationFSM
{
    public interface ICharacterBehavior
    {
        void Init(Animator anim, ICoroutineService coroutineService);
        void Enter();
        void Exit();
        void Update();
    }
}