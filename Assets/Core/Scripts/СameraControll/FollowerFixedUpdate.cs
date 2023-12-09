using UnityEngine;

namespace CameraControll
{
    public class FollowerFixedUpdate: Follower
    {
        private void FixedUpdate()
        {
            Move(Time.fixedDeltaTime);
        }
    }
}