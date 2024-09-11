using UnityEngine;

namespace Infrastructure.Services.Inputs
{
    public class PCInputDevice : MonoBehaviour, IInputDevice
    {
        public float Horizontal => Input.GetAxis("Horizontal");
        public float Vertical => Input.GetAxis("Vertical");
        public Vector2 Direction => new(Horizontal, Vertical);
    }
}