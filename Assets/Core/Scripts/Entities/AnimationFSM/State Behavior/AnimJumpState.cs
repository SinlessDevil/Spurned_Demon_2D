namespace Entities.AnimationFSM.StateBehavior
{
    public class AnimJumpState : AnimBehaviorState
    {
        public override void Enter()
        {
            _anim.SetBool(TypeAnimation.IsJumping.ToString(), true);
        }

        public override void Exit()
        {
            _anim.SetBool(TypeAnimation.IsJumping.ToString(), false);
        }
    }
}