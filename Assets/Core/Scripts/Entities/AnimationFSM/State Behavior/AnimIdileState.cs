namespace Entities.AnimationFSM.StateBehavior
{
    public sealed class AnimIdileState : AnimBehaviorState
    {
        public override void Enter()
        {
            _anim.SetBool(TypeAnimation.IsMove.ToString(), false);
            _anim.SetBool(TypeAnimation.IsAttack.ToString(), false);
            _anim.SetBool(TypeAnimation.IsJumping.ToString(), false);
        }
    }
}