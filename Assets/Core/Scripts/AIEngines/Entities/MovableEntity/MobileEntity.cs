using UnityEngine;
using Controller.Keyboard;
using Extensions;

namespace Entities.MovableEntity
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class MobileEntity : MonoBehaviour, IConrollable
    {
        [SerializeField] private Transform _bodyChaaracter;
        [Space(10)]
        [SerializeField] private Transform _chekerGroundPos;
        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private float _chekerRadius;

        private bool _isInitialize;
        
        protected float _moveSpeed;
        protected float _jumpHeight;
        
        private Rigidbody2D _rb;

        private float _moveInput;

        private Vector2 _lookRight;
        private Vector2 _lookLeft;

        private bool _isJump;
        protected bool _isGround;
        protected bool _isMoving;
        
        public virtual bool IsMoving { get => _isMoving; set => _isMoving = value; }
        public virtual bool IsJumping { get => _isJump; set => _isJump = value; }
        public virtual bool IsGround { get => _isGround; set => _isGround = value; }
        
        #region Init Methods
        public virtual void Initialize()
        {
            InitComponent();
            InitVectorLooks();

            Asserts();

            _isInitialize = true;
        }
        protected virtual void InitComponent()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        private void InitVectorLooks()
        {
            _lookRight = new Vector3(0, 0, 0);
            _lookLeft = new Vector3(0, 180, 0);
        }
        protected virtual void Asserts()
        {
            _bodyChaaracter.LogErrorIfComponentNull();
            _chekerGroundPos.LogErrorIfComponentNull();
        }
        #endregion

        private void FixedUpdate()
        {
            if (_isInitialize == false)
                return;

            IsGround = IsOnTheGround();
        }

        #region Controllable Methods
        public virtual void Jump() => _rb.velocity = Vector2.up * _jumpHeight;
        private bool IsOnTheGround() => Physics2D.OverlapCircle(_chekerGroundPos.position, _chekerRadius, _whatIsGround);

        public virtual void MoveTo(float moveInput)
        {
            _moveInput = moveInput;
            float moveAmount = _moveInput * _moveSpeed;
            transform.position += new Vector3(moveAmount * Time.deltaTime, 0, 0);
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