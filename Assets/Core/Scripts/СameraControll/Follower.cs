using UnityEngine;

namespace CameraControll
{
    public abstract class Follower : MonoBehaviour
    {
        private float _smoothing = 2f;
        private Vector3 _offset = new Vector3(0, 2, -10);
        private Transform _targetFollower;
        
        private bool _isFollowing = true;
        
        public Vector2 relativeBoundaryMin = new(-6.5f, -5);
        public Vector2 relativeBoundaryMax = new(6.5f, 5);

        public Vector2 innerBoundaryMin = new(-1, -1);
        public Vector2 innerBoundaryMax = new(1, 1);
        
        public void Initialize(Transform targetTransform, Vector3 offset, float smoothing)
        {
            _targetFollower = targetTransform;
            _offset = offset;
            _smoothing = smoothing;
        }

        protected void Move(float deltaTime)
        {
            Vector3 targetPosition = _targetFollower.position + _offset;
            Vector3 cameraPosition = transform.position;
            
            Vector3 minRelativeBoundary = cameraPosition + new Vector3(relativeBoundaryMin.x, relativeBoundaryMin.y, 0);
            Vector3 maxRelativeBoundary = cameraPosition + new Vector3(relativeBoundaryMax.x, relativeBoundaryMax.y, 0);
            bool canMoveToRelative = targetPosition.x < minRelativeBoundary.x || 
                                     targetPosition.x > maxRelativeBoundary.x ||
                                     targetPosition.y < minRelativeBoundary.y || 
                                     targetPosition.y > maxRelativeBoundary.y;

            Vector3 minInnerBoundary = cameraPosition + new Vector3(innerBoundaryMin.x, innerBoundaryMin.y, 0);
            Vector3 maxInnerBoundary = cameraPosition + new Vector3(innerBoundaryMax.x, innerBoundaryMax.y, 0);
            bool canMoveToInner = targetPosition.x < minInnerBoundary.x ||
                                  targetPosition.x > maxInnerBoundary.x ||
                                  targetPosition.y < minInnerBoundary.y || 
                                  targetPosition.y > maxInnerBoundary.y;
            
            if (canMoveToRelative || _isFollowing)
            {
                _isFollowing = true;
                
                if (canMoveToInner)
                {
                    targetPosition.x = Mathf.Lerp(targetPosition.x, Mathf.Clamp(targetPosition.x, innerBoundaryMin.x, innerBoundaryMax.x), deltaTime * _smoothing);
                    targetPosition.y = Mathf.Lerp(targetPosition.y, Mathf.Clamp(targetPosition.y, innerBoundaryMin.y, innerBoundaryMax.y), deltaTime * _smoothing);

                    var nextPosition = Vector3.Lerp(cameraPosition, targetPosition, deltaTime * _smoothing);
                    transform.position = nextPosition;
                }
                else
                {
                    _isFollowing = false;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Vector3 cameraPosition = transform.position;
    
            Vector3 minRelativeBoundary = cameraPosition + new Vector3(relativeBoundaryMin.x, relativeBoundaryMin.y, 0);
            Vector3 maxRelativeBoundary = cameraPosition + new Vector3(relativeBoundaryMax.x, relativeBoundaryMax.y, 0);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(minRelativeBoundary, new Vector3(maxRelativeBoundary.x, minRelativeBoundary.y, 0));
            Gizmos.DrawLine(new Vector3(maxRelativeBoundary.x, minRelativeBoundary.y, 0), maxRelativeBoundary);
            Gizmos.DrawLine(maxRelativeBoundary, new Vector3(minRelativeBoundary.x, maxRelativeBoundary.y, 0));
            Gizmos.DrawLine(new Vector3(minRelativeBoundary.x, maxRelativeBoundary.y, 0), minRelativeBoundary);
    
            Vector3 minInnerBoundary = cameraPosition + new Vector3(innerBoundaryMin.x, innerBoundaryMin.y, 0);
            Vector3 maxInnerBoundary = cameraPosition + new Vector3(innerBoundaryMax.x, innerBoundaryMax.y, 0);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(minInnerBoundary, new Vector3(maxInnerBoundary.x, minInnerBoundary.y, 0));
            Gizmos.DrawLine(new Vector3(maxInnerBoundary.x, minInnerBoundary.y, 0), maxInnerBoundary);
            Gizmos.DrawLine(maxInnerBoundary, new Vector3(minInnerBoundary.x, maxInnerBoundary.y, 0));
            Gizmos.DrawLine(new Vector3(minInnerBoundary.x, maxInnerBoundary.y, 0), minInnerBoundary);
        }
    }
}