namespace Entities.MovableEntity.Type
{
    public class Player : MobileEntity
    {
        public void InitConfig(float moveSpeed, float jumpHeight)
        {
            _moveSpeed = moveSpeed;
            _jumpHeight = jumpHeight;
        }
    }
}