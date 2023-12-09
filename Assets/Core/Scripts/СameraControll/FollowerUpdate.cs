using UnityEngine;

namespace CameraControll
{
    public class FollowerUpdate : Follower
    {
        private void Update()
        {
            Move(Time.deltaTime);
        }
    }
}