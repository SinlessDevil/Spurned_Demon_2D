namespace Entities.AnimationFSM.StateBehavior
{
    public class AnimAttackState : AnimBehaviorState
    {
        public override void Enter()
        {
            _anim.SetBool(TypeAnimation.IsAttack.ToString(), true);
        }

        public override void Exit()
        {
            _anim.SetBool(TypeAnimation.IsAttack.ToString(), false);
        }
    }
}