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

        public bool IsJumping { get => _isJump; set => _isJump = value; }
        public bool IsGound { get => _isGround; set => _isGround = value; }

        protected float _moveSpeed;
        protected float _jumpHeight;

        private bool _isJump;
        private bool _isGround;

        private Rigidbody2D _rb;

        private float _moveInput;

        private Vector2 _lookRight;
        private Vector2 _lookLeft;

        private bool _isInitialize;

        #region Init Methods
        public void Initialize()
        {
            InitComponent();
            InitVectorLooks();

            Asserts();

            _isInitialize = true;
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
        private void Asserts()
        {
            _bodyChaaracter.LogErrorIfComponentNull();
            _chekerGroundPos.LogErrorIfComponentNull();
        }
        #endregion

        private void FixedUpdate()
        {
            if (_isInitialize == false)
                return;

            _isGround = IsOnTheGround();
        }

        #region Controllable Methods
        public void Jump() => _rb.velocity = Vector2.up * _jumpHeight;
        private bool IsOnTheGround() => Physics2D.OverlapCircle(_chekerGroundPos.position, _chekerRadius, _whatIsGround);

        public void MoveTo(float moveInput)
        {
            _moveInput = moveInput;
            _rb.velocity = new Vector2(_moveInput * _moveSpeed, _rb.velocity.y);
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