namespace Entities.AnimationFSM.StateBehavior
{
    public class AnimDeadState : AnimBehaviorState
    {
        public override void Enter()
        {
            _anim.SetTrigger(TypeAnimation.IsDead.ToString());
        }
    }
}