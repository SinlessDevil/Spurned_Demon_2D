namespace Entities.AnimationFSM.StateBehavior
{
    public class AnimTakeDamageState : AnimBehaviorState
    {
        public override void Enter()
        {
            _anim.SetTrigger(TypeAnimation.IsTakeDamage.ToString());
        }
    }
}