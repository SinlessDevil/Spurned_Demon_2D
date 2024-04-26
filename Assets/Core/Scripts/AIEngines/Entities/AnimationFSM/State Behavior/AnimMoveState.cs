namespace Entities.AnimationFSM.StateBehavior
{
    public sealed class AnimMoveState : AnimBehaviorState
    {
        public override void Enter()
        {
            _anim.SetBool(TypeAnimation.IsMove.ToString(), true);
        }

        public override void Exit()
        {
            _anim.SetBool(TypeAnimation.IsMove.ToString(), false);
        }
    }
}