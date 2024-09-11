using UnityEngine;

namespace Infrastructure.Services.Inputs
{
    public class NullableInputDevice : IInputDevice
    {
        public float Horizontal { get; }
        public float Vertical { get; }
        public Vector2 Direction { get; }
    }
}