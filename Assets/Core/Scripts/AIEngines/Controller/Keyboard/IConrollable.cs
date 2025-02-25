namespace Controller.Keyboard
{
    public interface IConrollable
    {
        bool IsMoving { get; set; }
        bool IsJumping { get; set; }
        bool IsGround { get; }
        bool IsCanJump { get; }
        public void Jump();
        public void MoveTo(float direction);
        public void FlipBody(float moveInput);
    }
}