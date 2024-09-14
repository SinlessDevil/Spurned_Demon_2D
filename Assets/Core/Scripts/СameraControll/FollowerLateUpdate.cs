using UnityEngine;

namespace CameraControll
{
    public class FollowerLateUpdate: Follower
    {
        private void LateUpdate()
        {
            Move(Time.deltaTime);
        }
    }
}