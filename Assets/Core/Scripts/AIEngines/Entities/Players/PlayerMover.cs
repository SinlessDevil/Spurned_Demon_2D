using UnityEngine;
using Controller.Keyboard;
using Extensions;

namespace Core.Scripts.AIEngines.Entities.Players
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMover : MonoBehaviour, IConrollable
    {
        [SerializeField] private PlayerAnimatorViewer _playerAnimatorViewer;
        [SerializeField] private Transform _bodyChaaracter;
        [Space(10)]
        [SerializeField] private Transform _chekerGroundPos;
        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private float _chekerRadius;

        private bool _isInitialize;

        private bool _isGround;
        private bool _isMoving;
        
        private float _moveSpeed;
        private float _jumpHeight;
        
        private float _moveInput;

        private Vector2 _lookRight;
        private Vector2 _lookLeft;
        
        private Rigidbody2D _rb;
        
        public void Initialize()
        {
            InitComponent();
            InitVectorLooks();

            Asserts();

            _isInitialize = true;
        }

        public bool IsMoving
        {
            get => _isMoving;
            set
            {
                _isMoving = value;
                
                if(_isGround == false)
                    return;
                
                _playerAnimatorViewer.PlayMovingAnimation(_isMoving);
            }
        }
        public bool IsJumping { get; set; }
        public bool IsGround
        {
            get => _isGround;
            private set
            {
                _isGround = value;
                _playerAnimatorViewer.PlayJumpingAnimation(_isGround);
            }
        }
        
        public void InitConfig(float moveSpeed, float jumpHeight)
        {
            _moveSpeed = moveSpeed;
            _jumpHeight = jumpHeight;
        }
        
        private void InitComponent()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        private void InitVectorLooks()
        {
            _lookRight = new Vector3(0, 0, 0);
            _lookLeft = new Vector3(0, 180, 0);
        }
        
        private void FixedUpdate()
        {
            if (_isInitialize == false)
                return;

            IsGround = IsOnTheGround();
        }

        private void Asserts()
        {
            _bodyChaaracter.LogErrorIfComponentNull();
            _chekerGroundPos.LogErrorIfComponentNull();
        }
        
        #region Controllable Methods
        public virtual void Jump() => _rb.velocity = Vector2.up * _jumpHeight;
        private bool IsOnTheGround() => Physics2D.OverlapCircle(_chekerGroundPos.position, _chekerRadius, _whatIsGround);

        public void MoveTo(float moveInput)
        {
            _moveInput = moveInput;
            float moveAmount = _moveInput * _moveSpeed;
            _rb.velocity = new Vector2(moveAmount, _rb.velocity.y); 
        }
        public void FlipBody(float moveInput)
        {
            if (moveInput > 0)
                _bodyChaaracter.eulerAngles = _lookRight;
            else if (moveInput < 0)
                _bodyChaaracter.eulerAngles = _lookLeft;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_chekerGroundPos.position, _chekerRadius);
        }
        #endregion
    }
}