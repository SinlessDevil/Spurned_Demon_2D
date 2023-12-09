using UnityEngine;

namespace CameraControll
{
    public abstract class Follower : MonoBehaviour
    {
        [SerializeField] private float _smothing = 2f;
        
        private Vector3 _offset = new Vector3(0,2,-10);
        private Transform _targetFollower;
        
        public void Initialize(Transform targetTransfrom)
        {
            _targetFollower = targetTransfrom;
        }

        protected void Move(float deltaTime)
        {
            var nextPosition = Vector3.Lerp(transform.position, _targetFollower.position + _offset,
                deltaTime * _smothing);
            transform.position = nextPosition;
        }
    }
}