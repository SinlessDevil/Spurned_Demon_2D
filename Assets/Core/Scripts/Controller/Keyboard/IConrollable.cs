namespace Controller.Keyboard
{
    public interface IConrollable
    {
        bool IsJumping { get; set; }
        bool IsGound { get; }
        public void Jump();
        public void MoveTo(float direction);
        public void FlipBody(float moveInput);
    }
}