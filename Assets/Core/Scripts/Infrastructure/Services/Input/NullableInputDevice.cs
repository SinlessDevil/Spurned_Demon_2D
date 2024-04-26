using UnityEngine;

namespace Infrastructure.Services.Input
{
    public class NullableInputDevice : IInputDevice
    {
        public float Horizontal { get; }
        public float Vertical { get; }
        public Vector2 Direction { get; }
    }
}